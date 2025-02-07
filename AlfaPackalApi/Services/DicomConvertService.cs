using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto.DicomWeb;
using Api_PACsServer.Models.Dto.DicomWeb.Stow;
using Api_PACsServer.Services.IService.Dicom;
using Api_PACsServer.Utilities;
using AutoMapper;
using FellowOakDicom;
using FellowOakDicom.Serialization;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Reflection.PortableExecutable;

namespace Api_PACsServer.Services
{
    public class DicomConvertService : IDicomConvertService
    {
        private readonly IMapper _mapper;

        public DicomConvertService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<StowRsRequestDto> ParseStowRsRequestToDto(HttpRequest request)
        {
            var result = new StowRsRequestDto();
            var boundary = DicomWebHelper.GetBoundaryFromContentType(request.ContentType);
            // Create a MultipartReader to read each section of the multipart content
            var reader = new MultipartReader(boundary, request.Body);
            MultipartSection section;

            var metadataList = new List<MetadataDto>();
            var instancesList = new List<DicomFile>();
            long fileSize = 0; //

            string transactionUID = null;

            // Iterate through each section of the multipart content
            while ((section = await reader.ReadNextSectionAsync()) != null)
            {
                var contentType = section.ContentType;
                if (contentType == "application/dicom")
                {
                    using var memoryStream = new MemoryStream();
                    await section.Body.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    fileSize = memoryStream.Length;
                    var dicomFile = DicomFile.Open(memoryStream);
                    instancesList.Add(dicomFile);
                    // Extract TransactionUID from the DICOM file if not already set
                    if (transactionUID == null)
                    {
                        transactionUID = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.TransactionUID, null);
                    }
                }

                else if (contentType.StartsWith("application/dicom+json", StringComparison.OrdinalIgnoreCase))
                {
                    // Read the metadata JSON
                    using var readerStream = new StreamReader(section.Body);
                    var json = await readerStream.ReadToEndAsync();
                    var metadataDto = ConvertDicomJsonToMetadataDto(json);
                    metadataDto.TotalFileSizeMB = Math.Round((decimal)fileSize / (1024 * 1024), 2);
                    metadataList.Add(metadataDto);
                }

            }
            result.DicomFilesPackage = CombineMetadataAndDicomFiles(metadataList, instancesList);
            result.TransactionUID = transactionUID;
            return result;
        }

        private MetadataDto ConvertDicomJsonToMetadataDto(string dicomJson)
        {
            var dicomDataset = DicomJson.ConvertJsonToDicom(dicomJson);
            return _mapper.Map<MetadataDto>(dicomDataset);
            //return MapDicomDatasetToMetadataDto(dataset);
        }

        /// <summary>
        /// Combines metadata and DICOM files into a list of DicomFilePackage.
        /// </summary>
        /// <param name="metadataList">List of MetadataDto containing the metadata information.</param>
        /// <param name="dicomFiles">List of IFormFile representing the DICOM files.</param>
        /// <returns>A list of DicomFilePackage combining the metadata and DICOM files.</returns>
        internal static List<DicomFilePackage> CombineMetadataAndDicomFiles(List<MetadataDto> metadataList, List<DicomFile> dicomFiles)
        {
            var result = new List<DicomFilePackage>();

            if (metadataList.Count != dicomFiles.Count)
            {
                throw new InvalidOperationException("The number of metadata entries does not match the number of DICOM files.");
            }

            for (int i = 0; i < metadataList.Count; i++)
            {
                var stowRequest = new DicomFilePackage
                {
                    Metadata = metadataList[i],
                    DicomFile = dicomFiles[i],
                    //TotalFileSizeMB = Math.Round((decimal)instancesList[i].fileSize / (1024 * 1024), 2) // Convert bytes to MB
                };
                result.Add(stowRequest);
            };

            return result;
        }



        public async Task<string> ParseDicomResultToDicomJson(List<StowInstanceResult> operationResults, string transactionUID)
        {
            var acceptedInstances = operationResults
                .Where(r => r.IsSuccess)
                .Select(r => new ReferencedSOPInstance(r))
                .ToList();

            var failedInstances = operationResults
                .Where(r => !r.IsSuccess)
                .Select(r => new FailedInstance(r))
                .ToList();

            var response = new StowRsResponse(acceptedInstances, failedInstances);
            response.TransactionUID = transactionUID;
            // STATUS CONFIGURATION
            if (failedInstances.Count == 0 && acceptedInstances.Count > 0)
            {
                response.Status = "200 (OK)"; // All instances successfully stored
            }
            else if (failedInstances.Count > 0 && acceptedInstances.Count > 0)
            {
                response.Status = "202 (Accepted)"; // Some instances stored, some failed
            }
            else if (failedInstances.Count > 0 && acceptedInstances.Count == 0)
            {
                response.Status = "409 (Conflict)"; // All instances failed due to a conflict
            }
            else
            {
                response.Status = "400 (Bad Request)"; // Bad request, unable to store any instances
            }
            // Serialize the response to JSON
            return JsonConvert.SerializeObject(response);
        }




    }

}
