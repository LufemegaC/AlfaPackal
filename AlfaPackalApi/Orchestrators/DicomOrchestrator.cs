using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto.DicomWeb;
using Api_PACsServer.Models.Dto.DicomWeb.Qido;
using Api_PACsServer.Models.Dto.DicomWeb.Stow;
using Api_PACsServer.Models.Dto.Instances;
using Api_PACsServer.Models.Dto.Series;
using Api_PACsServer.Models.Dto.Studies;
using Api_PACsServer.Orchestrators.IOrchestrator;
using Api_PACsServer.Services.IService.Dicom;
using Api_PACsServer.Services.IService.MainEntities;
using Api_PACsServer.Utilities;
using FellowOakDicom;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Api_PACsServer.Orchestrators
{
    public class DicomOrchestrator : IDicomOrchestrator
    {
        private readonly IStudyService _studyService;
        private readonly ISerieService _serieService;
        private readonly IInstanceService _instanceService;
        private readonly IDicomFileService _azureDicomFileService;
        private readonly IDicomConvertService _dicomConvertService;

        public DicomOrchestrator(IStudyService studyService, ISerieService serieService,
                                    IInstanceService instanceService, IDicomFileService azureDicomFileService,
                                    IDicomConvertService dicomConvertService)
        {
            _studyService = studyService;
            _serieService = serieService;
            _instanceService = instanceService;
            _azureDicomFileService = azureDicomFileService;
            _dicomConvertService = dicomConvertService;
        }

        //  *** QIDO-RS *** //

        // Study Resource
        public async Task<string> GetAllStudies(IQueryCollection queryParams)
        {
            // Convertir la colección a un diccionario, uniendo los valores duplicados en listas
            var groupedParams = OrganizeParams(queryParams);
            // Inicializar queryParameters como null
            QueryRequestParameters<StudyQueryParametersDto>? queryParameters = null;

            // Dividir los parámetros en DTOs correspondientes si existen
            if (groupedParams != null)
            {
                queryParameters = DicomWebHelper.TranslateRequestParameters<StudyQueryParametersDto>(groupedParams);
            }

            // Obtener la lista de estudios utilizando el único parámetro queryParameters
            var studiesList = await _studyService.GetStudyData(queryParameters);

            return DicomWebHelper.ConvertDicomDtosToDicomJsonString(studiesList);
        }


        public async Task<string> GetAllSeriesFromStudy(string studyInstanceUID, IQueryCollection queryParams)
        {
            //Validate study info
            if (await _studyService.ExistsByUID(studyInstanceUID))
            {
                // Convertir la colección a un diccionario, uniendo los valores duplicados en listas
                var groupedParams = OrganizeParams(queryParams);
                if (!groupedParams.ContainsKey("StudyInstanceUID"))
                {
                    // Si no existe, agrega la clave con un nuevo List<string?>
                    groupedParams["StudyInstanceUID"] = new List<string?> { studyInstanceUID };
                }
                else
                {
                    // Si ya existe, agrega el StudyInstanceUID al listado correspondiente
                    groupedParams["StudyInstanceUID"].Add(studyInstanceUID);
                }
                // Inicializar queryParameters como null
                QueryRequestParameters<SerieQueryParametersDto>? queryParameters = null;
                queryParameters = DicomWebHelper.TranslateRequestParameters<SerieQueryParametersDto>(groupedParams);
                var seriesList = await _serieService.GetSeriesFromStudy(queryParameters);
                return DicomWebHelper.ConvertDicomDtosToDicomJsonString(seriesList);
            }
            else
                throw new KeyNotFoundException("StudyInstanceUID not found.");
        }

        public async Task<string> GetAllInstancesFromSeries(string studyInstanceUID, string seriesInstanceUID, IQueryCollection queryParams)
        {
            if (await _studyService.ExistsByUID(studyInstanceUID))
            {
                if(await _serieService.ExistsByUID(seriesInstanceUID))
                {
                    // Convertir la colección a un diccionario, uniendo los valores duplicados en listas
                    var groupedParams = OrganizeParams(queryParams);
                    if (!groupedParams.ContainsKey("SerieInstanceUID"))
                    {
                        // Si no existe, agrega la clave con un nuevo List<string?>
                        groupedParams["SerieInstanceUID"] = new List<string?> { studyInstanceUID };
                    }
                    else
                    {
                        // Si ya existe, agrega el StudyInstanceUID al listado correspondiente
                        groupedParams["SerieInstanceUID"].Add(seriesInstanceUID);
                    }
                    // Inicializar queryParameters como null
                    QueryRequestParameters<InstanceQueryParametersDto>? queryParameters = null;
                    queryParameters = DicomWebHelper.TranslateRequestParameters<InstanceQueryParametersDto>(groupedParams);
                    var InstanceList = await _instanceService.GetInstancesFromSerie(queryParameters);
                    return DicomWebHelper.ConvertDicomDtosToDicomJsonString(InstanceList);

                }
                else
                    throw new KeyNotFoundException("SeriesInstanceUID not found.");
            }
            else
                throw new KeyNotFoundException("StudyInstanceUID not found.");
        }
        // PRIVATE METHODS //
        private Dictionary<string,List<string?>> OrganizeParams(IQueryCollection queryParams)
        {
            return queryParams
                .ToDictionary(
                    group => group.Key,
                    group => group.Value.ToList()
                );

        }
        //  *** QIDO-RS *** //

        //  *** STOW-RS *** //



        /// <summary>
        /// ** NEW VERSION
        /// </summary>
        /// <param name="dicomFilesPackages"></param>
        /// <returns></returns>

        public async Task<string> ProcessStowRsRequest(HttpRequest httpRequest, string referencedStudyUID)
        {
            // Convertir a Dto
            var stowRsRequestDto = await _dicomConvertService.ParseStowRsRequestToDto(httpRequest);
            // Registro de entidades 
            var result = await RegisterDicomInstances(stowRsRequestDto.DicomFilesPackage,referencedStudyUID);
            // Convertir 
            return await _dicomConvertService.ParseDicomResultToDicomJson(result, stowRsRequestDto.TransactionUID);
        }



        /// <summary>
        /// ** OLD VERSION
        /// </summary>
        /// <param name="dicomFilesPackages"></param>
        /// <returns></returns>
        private async Task<List<StowInstanceResult>> RegisterDicomInstances(List<DicomFilePackage> dicomFilesPackages, string referencedStudyUID = null)
        {
            // 02/05/2025 !!!!!

            // PUNTO IMPORTANTE

            /*SE PAUSA DESARROLLO DEBIDO AL CAMBIO DE ENFOQUE EN LA ESTRUCTURA DEL LA SOLICITUD
             STOW-RS, SE TIENE QUE REALIZAR LOS CAMBIOS EN EL SERVIDOR LOCAL Y DESPUES VOLVERÉ */



            var operationResults = new List<StowInstanceResult>();
            var referencedStudy = new Study();

            foreach (var dicomPackage in dicomFilesPackages)
            {
                // Retrieve values from the DICOM instance
                string sopInstanceUID = dicomPackage.DicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.SOPInstanceUID, null);
                string sopClassUID = dicomPackage.DicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.SOPClassUID, null);
                if (!ValidateDicomPackage(dicomPackage))
                    ProcessInstanceResult(
                        operationResults,
                        StowInstanceResult.ProcessingFailure(sopClassUID, sopInstanceUID,"Instance and metadata not match"),
                        dicomFilesPackages,
                        dicomPackage
                        );
            }


            if (referencedStudyUID != null)
            {
                // Global validation
                if (DicomUID.IsValidUid(referencedStudyUID))
                    throw new InvalidOperationException("Referenced UID not valid");
                // referenced UID Validation
                referencedStudy = await _studyService.GetByUID(referencedStudyUID);
            }
            else
            {
                referencedStudy = null;
            }

            foreach (var dicomPackage in dicomFilesPackages)
            {
                // Extract UIDs
                var extractedStudyInstanceUID = dicomPackage.DicomFile.Dataset.GetString(DicomTag.StudyInstanceUID);
                var extractedSeriesInstanceUID = dicomPackage.DicomFile.Dataset.GetString(DicomTag.SeriesInstanceUID);
                var extractedSOPInstanceUID = dicomPackage.DicomFile.Dataset.GetString(DicomTag.SOPInstanceUID);
                // Validate UIDs
                if (dicomPackage.Metadata.StudyInstanceUID != extractedStudyInstanceUID ||
                    dicomPackage.Metadata.SeriesInstanceUID != extractedSeriesInstanceUID ||
                    dicomPackage.Metadata.SOPInstanceUID != extractedSOPInstanceUID)
                {
                    
                }




            }
            

            foreach (var dicomPackage in dicomFilesPackages)
            {
                //DicomFile dicomFile = null; // Declare dicomFile here to access it in the catch block
                try
                {
                    // Extract UIDs
                    var extractedStudyInstanceUID = dicomPackage.DicomFile.Dataset.GetString(DicomTag.StudyInstanceUID);
                    var extractedSeriesInstanceUID = dicomPackage.DicomFile.Dataset.GetString(DicomTag.SeriesInstanceUID);
                    var extractedSOPInstanceUID = dicomPackage.DicomFile.Dataset.GetString(DicomTag.SOPInstanceUID);
                    // Flags
                    bool dataCoerced = false;
                    // Validate UIDs
                    if (dicomPackage.Metadata.StudyInstanceUID != extractedStudyInstanceUID ||
                        dicomPackage.Metadata.SeriesInstanceUID != extractedSeriesInstanceUID ||
                        dicomPackage.Metadata.SOPInstanceUID != extractedSOPInstanceUID)
                    {
                        // Add failure result due to UID mismatch
                        AddFailureResult(operationResults,
                                "The provided UIDs do not match those in the DICOM file.",
                                extractedStudyInstanceUID,
                                extractedSeriesInstanceUID,
                                extractedSOPInstanceUID);
                        continue; // Skip to the next instance
                    }
                    // Storage Metadata
                    var metadataResult = await RegisterMetadata(dicomPackage.Metadata, ReferencedStudyUID);

                    if (!metadataResult.IsSuccess)
                    {
                        // Add failure result due to metadata registration error
                        AddFailureResult(operationResults,
                            metadataResult.ErrorMessage,
                            extractedStudyInstanceUID,
                            extractedSeriesInstanceUID,
                            extractedSOPInstanceUID);
                        continue; // Skip to the next instance
                    }

                    // Storage File
                    // NOT DELETE
                    //var storageResult = await _azureDicomFileService.StoreDicomFileAsync(
                    //    dicomPackage.DicomFile,
                    //    extractedStudyInstanceUID,
                    //    extractedSeriesInstanceUID,
                    //    extractedSOPInstanceUID
                    //);

                    //if (!storageResult.IsSuccess)
                    //{
                    //    // Add failure result due to storage error
                    //    AddFailureResult(operationResults,
                    //        storageResult.ErrorMessage,
                    //        extractedStudyInstanceUID,
                    //        extractedSeriesInstanceUID,
                    //        extractedSOPInstanceUID);
                    //    continue; // Skip to the next instance
                    //}

                    // Add success result
                    operationResults.Add(StowInstanceResult.Success(
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
                        studyInstanceUID = dicomPackage.DicomFile.Dataset.GetString(DicomTag.StudyInstanceUID);
                        seriesInstanceUID = dicomPackage.DicomFile?.Dataset.GetString(DicomTag.SeriesInstanceUID);
                        sopInstanceUID = dicomPackage.DicomFile?.Dataset.GetString(DicomTag.SOPInstanceUID);
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

        internal async Task<StowInstanceResult> RegisterMetadata(MetadataDto metadataDto, string referencedStudyUID)
        {
            try
            {
                //file size
                var fileSize = metadataDto.TotalFileSizeMB;
                // main UIDs
                var studyInstanceUID = metadataDto.StudyInstanceUID;
                var serieInstanceUID = metadataDto.SeriesInstanceUID;
                var sopInstanceUID = metadataDto.SOPInstanceUID;
                // Llamamos a ValidateStudyUIDs y desempaquetamos los resultados
                (ValidationResult validationResult, bool studyExists) = await ValidateStudyUIDs(studyInstanceUID, referencedStudyUID);
                // Si la validación falla, retornamos un fallo
                if (validationResult != ValidationResult.Success)
                {
                    return StowInstanceResult.


                    return OperationResult.Failure(validationResult.ErrorMessage);
                }
                bool seriesExists = await _serieService.ExistsByUID(serieInstanceUID);
                bool instanceExists = await _instanceService.ExistsByUID(sopInstanceUID);

                // Mapping create entitities
                var studyCreateDto = await _studyService.MapMetadataToCreateDto(metadataDto);
                var serieCreateDto = await _serieService.MapMetadataToCreateDto(metadataDto);
                var instanceCreateDto = await _instanceService.MapMetadataToCreateDto(metadataDto);

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
                    }
                    else
                    {
                        serie = await _serieService.GetByUID(serieInstanceUID);
                    }
                    // If the Instance does not exist, create it
                    Instance instance = await _instanceService.Create(instanceCreateDto);
                    await UpdateStudyAndSeries(studyExists ? studyInstanceUID : null, seriesExists ? serieInstanceUID : null, serie.Modality, fileSize);
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

        /// <summary>
        /// Processes a DICOM instance by adding its result to the result list
        /// and removing the processed package from the pending list.
        /// </summary>
        /// <param name="stowResults">List that stores results of processed instances.</param>
        /// <param name="instanceResult">The result object of the current DICOM instance being processed.</param>
        /// <param name="dicomPackages">List of pending DICOM packages to be processed.</param>
        /// <param name="dicomPackage">The specific DICOM package that has been processed and should be removed.</param>
        private void ProcessInstanceResult(
            List<StowInstanceResult> stowResults,
            StowInstanceResult instanceResult,
            List<DicomFilePackage> dicomPackages,
            DicomFilePackage dicomPackage)
        {
            // Add the processed instance result to the list
            stowResults.Add(instanceResult);

            // Remove the processed DICOM package from the pending list
            dicomPackages.Remove(dicomPackage);
        }



        /// <summary>
        /// Validates if the DICOM metadata matches the instance data.
        /// Ensures consistency between the metadata and the actual DICOM instance.
        /// </summary>
        /// <param name="dicomPackage">The DICOM package containing metadata and instance data.</param>
        /// <returns>True if metadata matches the instance, otherwise false.</returns>
        private bool ValidateDicomPackage(DicomFilePackage dicomPackage)
        {
            if (dicomPackage.Metadata == null || dicomPackage.DicomFile == null)
                return false; // Cannot validate if there is missing data

            var dataset = dicomPackage.DicomFile.Dataset;

            // Retrieve values from the DICOM instance
            string dicomSOPInstanceUID = dataset.GetSingleValueOrDefault<string>(DicomTag.SOPInstanceUID, null);
            string dicomSOPClassUID = dataset.GetSingleValueOrDefault<string>(DicomTag.SOPClassUID, null);
            string dicomSeriesInstanceUID = dataset.GetSingleValueOrDefault<string>(DicomTag.SeriesInstanceUID, null);
            string dicomStudyInstanceUID = dataset.GetSingleValueOrDefault<string>(DicomTag.StudyInstanceUID, null);
            string dicomModality = dataset.GetSingleValueOrDefault<string>(DicomTag.Modality, null);

            // Validate metadata matches the instance
            if (dicomSOPInstanceUID != dicomPackage.Metadata.SOPInstanceUID)
                return false;

            if (!string.IsNullOrEmpty(dicomPackage.Metadata.SOPClassUID) && dicomSOPClassUID != dicomPackage.Metadata.SOPClassUID)
                return false;

            if (!string.IsNullOrEmpty(dicomPackage.Metadata.SeriesInstanceUID) && dicomSeriesInstanceUID != dicomPackage.Metadata.SeriesInstanceUID)
                return false;

            if (!string.IsNullOrEmpty(dicomPackage.Metadata.StudyInstanceUID) && dicomStudyInstanceUID != dicomPackage.Metadata.StudyInstanceUID)
                return false;

            if (!string.IsNullOrEmpty(dicomPackage.Metadata.Modality) && dicomModality != dicomPackage.Metadata.Modality)
                return false;

            return true;
        }


        /// <summary>
        /// Validates the provided StudyInstanceUID against the ReferencedStudyInstanceUID, if provided.
        /// If the ReferencedStudyInstanceUID is not null or empty, it ensures that it matches the StudyInstanceUID.
        /// Additionally, it checks whether the StudyInstanceUID exists in the system.
        /// </summary>
        /// <param name="studyInstanceUID">The StudyInstanceUID of the DICOM instance being processed.</param>
        /// <param name="referencedStudyUID">The optional ReferencedStudyInstanceUID that should match the StudyInstanceUID if provided.</param>
        /// <returns>
        /// A <see cref="Tuple{ValidationResult, bool}"/> where the first value is a <see cref="ValidationResult"/> indicating success or failure,
        /// and the second value is a boolean indicating whether the StudyInstanceUID exists in the system.
        /// If the ReferencedStudyInstanceUID is provided and does not match the StudyInstanceUID, the method returns a failure message.
        /// If the StudyInstanceUID exists, it returns success along with true. If it does not exist, and the ReferencedStudyInstanceUID is provided,
        /// it returns a failure message. Otherwise, it returns success with false.
        /// </returns>
        private async Task<Tuple<ValidationResult, bool>> ValidateStudyUIDs(string studyInstanceUID, string referencedStudyUID)
        {
            bool IsReferenced = !string.IsNullOrEmpty(referencedStudyUID);
            if (IsReferenced && (studyInstanceUID != referencedStudyUID) )
            {
                return Tuple.Create(new ValidationResult("Referenced StudyInstanceUID does not match the instance's StudyInstanceUID."), false);
            }

            if(await _studyService.ExistsByUID(studyInstanceUID))
            {
                return Tuple.Create(ValidationResult.Success, true);
            }
            else if (IsReferenced)
            {
                return Tuple.Create(new ValidationResult("Referenced StudyInstanceUID not found."), false);
            }
            else
            {
                return Tuple.Create(ValidationResult.Success, false);
            }
        }

        private async Task UpdateStudyAndSeries(string? studyInstanceUID, string? SeriesInstanceUID, string? modality, decimal fileSize)
        {
            if (studyInstanceUID != null)  //Study already exists
            { 
                if (SeriesInstanceUID == null) // new instance
                    await _studyService.UpdateForNewSerie(studyInstanceUID, modality);
                else
                    await _serieService.UpdateDetailsForNewInstance(SeriesInstanceUID, fileSize);
                await _studyService.UpdateDetailsForNewInstance(studyInstanceUID, fileSize);
            }
        }

        
        //  *** STOW-RS *** //

    }
}
