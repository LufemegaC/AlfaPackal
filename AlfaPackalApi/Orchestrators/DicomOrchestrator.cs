using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Models.Dto.DicomWeb;
using Api_PACsServer.Orchestrators.IOrchestrator;
using Api_PACsServer.Services.IService.Dicom;
using Api_PACsServer.Services.IService.Pacs;
using FellowOakDicom;

namespace Api_PACsServer.Orchestrators
{
    public class DicomOrchestrator : IDicomOrchestrator
    {
        private readonly IStudyService _studyService;
        private readonly ISerieService _serieService;
        private readonly IInstanceService _instanceService;
        private readonly IDicomFileService _azureDicomFileService;


        public DicomOrchestrator(IStudyService studyService, ISerieService serieService,
                                    IInstanceService instanceService, IDicomFileService azureDicomFileService)
        {
            _studyService = studyService;
            _serieService = serieService;
            _instanceService = instanceService;
            _azureDicomFileService = azureDicomFileService;
        }


        /// <remarks>
        /// Pendiente :
        /// 1.- Cambiar el retorno de las main por solo el PACSId ( cancelado )
        /// </remarks>
        public async Task<OperationResult> RegisterMainEntities(MainEntitiesCreateDto mainEntitiesCreate)
        {
            try
            {
                //file size
                var fileSize = mainEntitiesCreate.TotalFileSizeMB;
                // main UIDs
                var studyInstanceUID = mainEntitiesCreate.StudyInstanceUID;
                var serieInstanceUID = mainEntitiesCreate.SeriesInstanceUID;
                var sopInstanceUID = mainEntitiesCreate.SOPInstanceUID;

                // Check if entities exists
                bool studyExists = await _studyService.ExistsByUID(studyInstanceUID);
                bool seriesExists = await _serieService.ExistsByUID(serieInstanceUID);
                bool instanceExists = await _instanceService.ExistsByUID(sopInstanceUID);

                // Mapping create entitities
                var studyCreateDto = await _studyService.MapToCreateDto(mainEntitiesCreate);
                var serieCreateDto = await _serieService.MapToCreateDto(mainEntitiesCreate);
                var instanceCreateDto = await _instanceService.MapToCreateDto(mainEntitiesCreate);
               
                //**
                if (!instanceExists)
                {
                    // If the Study does not exist, create it
                    Study study;
                    if (!studyExists)
                    {
                        study = await _studyService.Create(studyCreateDto);
                    }
                    else
                    {
                        study = await _studyService.GetByUID(studyInstanceUID);
                    }
                    // If the Series does not exist, create it
                    Serie serie;
                    if (!seriesExists)
                    {
                        //serieCreateDto.StudyID = study.StudyID;
                        serie = await _serieService.Create(serieCreateDto);
                        if (studyExists)
                        {
                            await _studyService.UpdateDetailsForNewSerie(study.StudyInstanceUID);
                        }
                    }
                    else
                    {
                        serie = await _serieService.GetByUID(serieInstanceUID);
                    }
                    // If the Instance does not exist, create it
                    Instance instance = await _instanceService.Create(instanceCreateDto);
                    if (studyExists)
                    {
                        await _studyService.UpdateDetailsForNewInstance(study.StudyInstanceUID, fileSize);
                    }
                    if (seriesExists)
                    {
                        await _serieService.UpdateLoadForNewInstance(serie.SeriesInstanceUID, fileSize);
                    }
                    return OperationResult.Success();
                }
                {
                    return OperationResult.Warning("SOP Instance UID already exists.");
                }
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Error during registration: {ex.Message}");
            }
            
        }

        public UserStudiesListDto GetRecentStudies(PaginationParameters parameters)
        {
            return _studyService.GetRecentStudies(parameters);
        }

        public async Task<OperationResult> GetInfoStudy(string studyInstanceUID)
        {
            try
            {
                var studyExists = await _studyService.ExistsByUID(studyInstanceUID);
                //**
                if (!studyExists)
                {
                    return OperationResult.Failure("Study not found by UID.");
                }
                // Retrieve the study mapped as an OHIFStudy object
                var studyOHIF = await _studyService.GetOHIFByUID(studyInstanceUID);
                // Retrieve all series associated with the study
                //var serie01 = await _serieService.GetAllByStudyUID("1.2.392.200036.9125.2.100010611365144.6578282807.3956157");
                var seriesList = await _serieService.GetAllByStudyUID(studyInstanceUID);
                // Convert the series collection from IEnumerable to List and assign to OHIFStudy
                studyOHIF.Series = seriesList.ToList();
                // Loop through each series to retrieve its associated instances
                foreach (var serie in studyOHIF.Series)
                {
                    var instanceList = await _instanceService.GetAllBySerieUID(serie.SeriesInstanceUID);
                    // Convert the instance collection from IEnumerable to List and assign to the corresponding series
                    serie.Instances = instanceList.ToList();
                }
                // Return the fully populated study with its series and instances
                return OperationResult.Success(studyOHIF);
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Error during registration: {ex.Message}");
            }

        }

        /// <summary>
        /// ** NEW VERSION FOR REGISTER DICOM OBJECT
        /// </summary>
        /// <param name="RequestDto"></param>
        /// <returns></returns>
        public async Task<List<DicomOperationResult>> RegisterDicomInstances(List<StowRsRequestDto> requestsDto)
        {
            var operationResults = new List<DicomOperationResult>();

            foreach (var requestDto in requestsDto)
            {
                DicomFile dicomFile = null; // Declare dicomFile here to access it in the catch block

                try
                {
                    //** Read DICOM file
                    try
                    {
                        
                        using (var stream = requestDto.DicomFile.OpenReadStream())
                        {
                            dicomFile = DicomFile.Open(stream);
                        }
                    }
                    catch (DicomFileException ex)
                    {
                        operationResults.Add(DicomOperationResult.Failure(
                            "Cannot parse DICOM file.",
                            null, null, null,
                            49152 // Cannot Understand
                        ));
                        continue;
                    }

                    // Extract UIDs
                    var extractedStudyInstanceUID = dicomFile.Dataset.GetString(DicomTag.StudyInstanceUID);
                    var extractedSeriesInstanceUID = dicomFile.Dataset.GetString(DicomTag.SeriesInstanceUID);
                    var extractedSOPInstanceUID = dicomFile.Dataset.GetString(DicomTag.SOPInstanceUID);
                    // Flags
                    bool dataCoerced = false;

                    // Validate UIDs
                    if (requestDto.Metadata.StudyInstanceUID != extractedStudyInstanceUID ||
                        requestDto.Metadata.SeriesInstanceUID != extractedSeriesInstanceUID ||
                        requestDto.Metadata.SOPInstanceUID != extractedSOPInstanceUID)
                    {
                        // Add failure result due to UID mismatch
                        AddFailureResult(operationResults,
                            "The provided UIDs do not match those in the DICOM file.",
                            extractedStudyInstanceUID,
                            extractedSeriesInstanceUID,
                            extractedSOPInstanceUID);
                        continue; // Skip to the next instance
                    }

                    // ** PENDIENTE IMPLEMENTACION - VALIDAR ESPACIO DISPONIBLE
                    //var storageResult = await _azureDicomFileService.(requestDto.Metadata.TotalFileSizeMB); );
                    //if (!storageResult.IsSuccess)
                    //{
                    //    // Add failure result due to storage error
                    //    AddFailureResult(operationResults,
                    //        storageResult.ErrorMessage,
                    //        extractedStudyInstanceUID,
                    //        extractedSeriesInstanceUID,
                    //        extractedSOPInstanceUID,
                    //        42752); // Out of Resources);
                    //    continue; // Skip to the next instance
                    //}

                    // ** PENSIENTE IMPLEMENTACION - VALIDACION DE SOPORTE DE SOPCLASSUID
                    // Validate SOP Class UID
                    //var sopClassUID = dicomFile.Dataset.GetString(DicomTag.SOPClassUID);
                    //// Check if SOP Class is supported
                    //if (!SupportedSopClasses.Contains(sopClassUID))
                    //{
                    //    operationResults.Add(DicomOperationResult.Failure(
                    //        "Unsupported SOP Class.",
                    //        extractedStudyInstanceUID,
                    //        extractedSeriesInstanceUID,
                    //        extractedSOPInstanceUID,
                    //        43264 // Data Set Does Not Match SOP Class
                    //    ));
                    //    continue;
                    //}

                    // ** PENSIENTE IMPLEMENTACION - VALIDACION DE ATRIBUTOS SOPCLASSUID
                    // Validate required attributes for the SOP Class
                    //if (!AreRequiredAttributesPresent(dicomFile.Dataset, sopClassUID))
                    //{
                    //    operationResults.Add(DicomOperationResult.Failure(
                    //        "Missing required attributes for the SOP Class.",
                    //        extractedStudyInstanceUID,
                    //        extractedSeriesInstanceUID,
                    //        extractedSOPInstanceUID,
                    //        43264 // Data Set Does Not Match SOP Class
                    //    ));
                    //    continue;
                    //}

                    // ** PENDIENTE IMPLEMENTACION - VALIDACION SOPORTE TRANSFER SYNTAX UID
                    // Validate Transfer Syntax
                    //var transferSyntaxUID = dicomFile.FileMetaInfo.TransferSyntaxUID.UID;
                    //if (!SupportedTransferSyntaxes.Contains(transferSyntaxUID))
                    //{
                    //    operationResults.Add(DicomOperationResult.Failure(
                    //        "Referenced Transfer Syntax not supported.",
                    //        extractedStudyInstanceUID,
                    //        extractedSeriesInstanceUID,
                    //        extractedSOPInstanceUID,
                    //        290 // Referenced Transfer Syntax Not Supported
                    //    ));
                    //    continue;
                    //}

                    // ** PENDIENTE IMPLEMENTACION - VALIDA SI ES NECESARIO AJUSTAR METADATOS
                    //if (NeedsModification(dicomFile.Dataset))
                    //{
                    //    CoerceDataElements(dicomFile.Dataset);
                    //    dataCoerced = true;
                    //}


                    // Storage Metadata
                    var metadataResult = await RegisterMetadata(requestDto.Metadata);

                    if (!metadataResult.IsSuccess)
                    {
                        // PENDIENTE 
                        //if (dataCoerced)  

                       // Add failure result due to metadata registration error
                        AddFailureResult(operationResults,
                            metadataResult.ErrorMessage,
                            extractedStudyInstanceUID,
                            extractedSeriesInstanceUID,
                            extractedSOPInstanceUID);
                        continue; // Skip to the next instance
                    }

                    // Storage File
                    var storageResult = await _azureDicomFileService.StoreDicomFileAsync(
                        requestDto.DicomFile,
                        extractedStudyInstanceUID,
                        extractedSeriesInstanceUID,
                        extractedSOPInstanceUID
                    );

                    if (!storageResult.IsSuccess)
                    {
                        // Add failure result due to storage error
                        AddFailureResult(operationResults,
                            storageResult.ErrorMessage,
                            extractedStudyInstanceUID,
                            extractedSeriesInstanceUID,
                            extractedSOPInstanceUID);
                        continue; // Skip to the next instance
                    }

                    // Add success result
                    operationResults.Add(DicomOperationResult.Success(
                        studyInstanceUID: extractedStudyInstanceUID,
                        seriesInstanceUID: extractedSeriesInstanceUID,
                        sopInstanceUID: extractedSOPInstanceUID
                    ));
                }
                catch (DicomFileException ex)
                {
                    AddFailureResult(operationResults,
                        $"Invalid DICOM file: {ex.Message}",
                        null, null, null,
                        failureReason: 49152 // Cannot Understand
                    );
                }
                catch (Exception ex)
                {
                    // Extract UIDs if possible
                    string studyInstanceUID = null;
                    string seriesInstanceUID = null;
                    string sopInstanceUID = null;

                    try
                    {
                        studyInstanceUID = dicomFile?.Dataset.GetString(DicomTag.StudyInstanceUID);
                        seriesInstanceUID = dicomFile?.Dataset.GetString(DicomTag.SeriesInstanceUID);
                        sopInstanceUID = dicomFile?.Dataset.GetString(DicomTag.SOPInstanceUID);
                    }
                    catch
                    {
                        // Ignore exceptions during UID extraction
                    }

                    AddFailureResult(operationResults,
                     $"Error during registration: {ex.Message}",
                     studyInstanceUID,
                     seriesInstanceUID,
                     sopInstanceUID);
                    continue;
                    
                }
            }

            // Return the list of operation results
            return operationResults;
        }

        internal async Task<OperationResult> RegisterMetadata(MetadataDto mainEntitiesCreate)
        {
            try
            {
                //file size
                var fileSize = mainEntitiesCreate.TotalFileSizeMB;
                // main UIDs
                var studyInstanceUID = mainEntitiesCreate.StudyInstanceUID;
                var serieInstanceUID = mainEntitiesCreate.SeriesInstanceUID;
                var sopInstanceUID = mainEntitiesCreate.SOPInstanceUID;

                // Check if entities exists
                bool studyExists = await _studyService.ExistsByUID(studyInstanceUID);
                bool seriesExists = await _serieService.ExistsByUID(serieInstanceUID);
                bool instanceExists = await _instanceService.ExistsByUID(sopInstanceUID);

                // Mapping create entitities
                var studyCreateDto = await _studyService.MapMetadataToCreateDto(mainEntitiesCreate);
                var serieCreateDto = await _serieService.MapMetadataToCreateDto(mainEntitiesCreate);
                var instanceCreateDto = await _instanceService.MapMetadataToCreateDto(mainEntitiesCreate);

                //**
                if (!instanceExists)
                {
                    // If the Study does not exist, create it
                    Study study;
                    if (!studyExists)
                    {
                        study = await _studyService.Create(studyCreateDto);
                    }
                    else
                    {
                        study = await _studyService.GetByUID(studyInstanceUID);
                    }
                    // If the Series does not exist, create it
                    Serie serie;
                    if (!seriesExists)
                    {
                        //serieCreateDto.StudyID = study.StudyID;
                        serie = await _serieService.Create(serieCreateDto);
                        if (studyExists)
                        {
                            await _studyService.UpdateDetailsForNewSerie(study.StudyInstanceUID);
                        }
                    }
                    else
                    {
                        serie = await _serieService.GetByUID(serieInstanceUID);
                    }
                    // If the Instance does not exist, create it
                    Instance instance = await _instanceService.Create(instanceCreateDto);
                    if (studyExists)
                    {
                        await _studyService.UpdateDetailsForNewInstance(study.StudyInstanceUID, fileSize);
                    }
                    if (seriesExists)
                    {
                        await _serieService.UpdateLoadForNewInstance(serie.SeriesInstanceUID, fileSize);
                    }
                    return OperationResult.Success();
                }
                {
                    return OperationResult.Warning("SOP Instance UID already exists.");
                }
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Error during registration: {ex.Message}");
            }

        }

        // Add error instance to List
        private void AddFailureResult(
            List<DicomOperationResult> operationResults,
            string errorMessage,
            string studyInstanceUID,
            string seriesInstanceUID,
            string sopInstanceUID,
            int failureReason = 272)
        {
            operationResults.Add(DicomOperationResult.Failure(
                errorMessage: errorMessage,
                studyInstanceUID: studyInstanceUID,
                seriesInstanceUID: seriesInstanceUID,
                sopInstanceUID: sopInstanceUID,
                failureReason: failureReason
            ));
        }
    }
}
