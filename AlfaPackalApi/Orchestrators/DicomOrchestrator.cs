using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Models.Dto.DicomWeb;
using Api_PACsServer.Models.Dto.Studies;
using Api_PACsServer.Orchestrators.IOrchestrator;
using Api_PACsServer.Services.IService.Dicom;
using Api_PACsServer.Services.IService.Pacs;
using FellowOakDicom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        //  *** QIDO-RS *** //


        public async Task<List<StudyDto>> GetInfoStudy(StudyQueryParametersDto studyQuery, ControlQueryParametersDto controlQuery)
        {
            // Evaluar si se debe ejecutar una consulta de paginación estándar
            if (IsPaginationOnlyQuery(studyQuery, controlQuery))
            {
                var studies = await _studyService.AllStudiesByControlParams(controlQuery);
                return studies;
            }


            throw new NotImplementedException();
        }


        private bool IsPaginationOnlyQuery(StudyQueryParametersDto studyParamsDto, ControlQueryParametersDto controlParamsDto)
        {
            // validate atributes
            bool hasStudySpecificParams = !string.IsNullOrEmpty(studyParamsDto.PatientName.Value) ||
                                          !string.IsNullOrEmpty(studyParamsDto.StudyDate.Value) ||
                                          !string.IsNullOrEmpty(studyParamsDto.AccessionNumber.Value);

            bool hasPaginationParams = !string.IsNullOrEmpty(controlParamsDto.Page.Value) && !string.IsNullOrEmpty(controlParamsDto.PageSize.Value) &&
                                       !string.IsNullOrEmpty(controlParamsDto.OrderBy.Value);
            
            // check values
            if (int.Parse(controlParamsDto.Page.Value) <= 0)
                throw new ArgumentException("Page number must be greater than zero.");
            
            if (int.Parse(controlParamsDto.PageSize.Value) <= 0)
                throw new ArgumentException("Page size must be greater than zero.");

            // Si no hay parámetros específicos del estudio y hay parámetros de paginación, entonces es una consulta estándar de paginación
            return !hasStudySpecificParams && hasPaginationParams;
        }

        //  *** QIDO-RS *** //

        //  *** STOW-RS *** //

        /// <summary>
        /// ** NEW VERSION FOR REGISTER DICOM OBJECT
        /// </summary>
        /// <param name="dicomFilesPackages"></param>
        /// <returns></returns>
        public async Task<List<DicomOperationResult>> RegisterDicomInstances(List<DicomFilePackage> dicomFilesPackages, string ReferencedStudyUID = null)
        {
            var operationResults = new List<DicomOperationResult>();

            foreach (var dicomPackage in dicomFilesPackages)
            {
                //DicomFile dicomFile = null; // Declare dicomFile here to access it in the catch block
                try
                {
                    ////** Read DICOM file
                    //try
                    //{
                    //    dicomPackage.DicomFile.Position = 0;

                    //    dicomPackage.DicomFile.

                    //    if (dicomPackage.DicomFile == null || dicomPackage.DicomFile.Length == 0)
                    //    {
                    //        operationResults.Add(DicomOperationResult.Failure(
                    //            "DICOM file is missing or empty.",
                    //            null, null, null,
                    //            49152 // Cannot Understand
                    //        ));
                    //        continue;
                    //    }

                    //    using (var memoryStream = new MemoryStream())
                    //    {
                    //        await dicomPackage.DicomFile.CopyToAsync(memoryStream);
                    //        memoryStream.Position = 0; // Reiniciar la posición para la lectura

                    //        dicomFile = DicomFile.Open(memoryStream);
                    //    }
                    //}
                    //catch (DicomFileException ex)
                    //{
                    //    operationResults.Add(DicomOperationResult.Failure(
                    //        "Cannot parse DICOM file.",
                    //        null, null, null,
                    //        49152 // Cannot Understand
                    //    ));
                    //    continue;
                    //}

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

        internal async Task<OperationResult> RegisterMetadata(MetadataDto metadataDto, string referencedStudyUID)
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
                    await UpdateStudyAndSeriesDetails(study, serie, fileSize);
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
        private void AddFailureResult(List<DicomOperationResult> operationResults, string errorMessage,
             string studyInstanceUID, string seriesInstanceUID, string sopInstanceUID, int failureReason = 272)
        {
            operationResults.Add(DicomOperationResult.Failure(
                errorMessage: errorMessage,
                studyInstanceUID: studyInstanceUID,
                seriesInstanceUID: seriesInstanceUID,
                sopInstanceUID: sopInstanceUID,
                failureReason: failureReason
            ));
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


        private async Task UpdateStudyAndSeriesDetails(Study study, Serie serie, decimal fileSize)
        {
            if (study != null)
            {
                await _studyService.UpdateDetailsForNewInstance(study.StudyInstanceUID, fileSize);
            }
            if (serie != null)
            {
                await _serieService.UpdateLoadForNewInstance(serie.SeriesInstanceUID, fileSize);
            }
        }

        //  *** STOW-RS *** //




    }
}
