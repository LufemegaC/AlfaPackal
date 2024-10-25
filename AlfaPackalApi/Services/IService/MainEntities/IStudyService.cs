using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Modelos.Load;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Models.Dto.Instances;
using Api_PACsServer.Models.Dto.Series;
using Api_PACsServer.Models.Dto.Studies;
using Api_PACsServer.Models.OHIFVisor;

namespace Api_PACsServer.Services.IService.Pacs
{
    /// <summary>
    /// Service interface for handling Study-related operations in the PACS system.
    /// </summary>
    public interface IStudyService
    {
        /// <summary>
        /// Creates a new Study entity.
        /// </summary>
        /// <param name="CreateDto">The data transfer object containing the details of the Study to be created.</param>
        /// <returns>The task result contains the created Study entity.</returns>
        Task<Study> Create(StudyCreateDto CreateDto);

        /// <summary>
        /// Gets a Study entity by its ID.
        /// </summary>
        /// <param name="studyId">The ID of the Study to be retrieved.</param>
        /// <returns>The task result contains the Study entity.</returns>
        Task<Study> GetById(int studyId);

        /// <summary>
        /// Gets a Study entity by its UID.
        /// </summary>
        /// <param name="studyInstanceUID">The UID of the Study to be retrieved.</param>
        /// <returns>The task result contains the Study entity.</returns>
        Task<Study> GetByUID(string studyInstanceUID);


        Task<OHIFStudy> GetOHIFByUID(string studyInstanceUID);

        /// <summary>
        /// Checks if a Study entity exists by its UID.
        /// </summary>
        /// <param name="studyInstanceUID">The UID of the Study to be checked.</param>
        /// <returns>The task result contains a boolean indicating whether the Study exists.</returns>
        Task<bool> ExistsByUID(string studyInstanceUID);

        /// <summary>
        /// Updates the load information of a Study entity for a new series.
        /// </summary>
        /// <param name="studyInstanceUID">The UID of the Study to be updated.</param>
        /// <returns>The task result contains the updated StudyLoad entity.</returns>
        Task<StudyDetails> UpdateDetailsForNewSerie(string studyInstanceUID);

        /// <summary>
        /// Updates the load information of a Study entity for a new instance.
        /// </summary>
        /// <param name="studyInstanceUID">The UID of the Study to be updated.</param>
        /// <param name="totalSizeFile">The total size of the file in MB.</param>
        /// <returns>The task result contains the updated StudyLoad entity.</returns>
        Task<StudyDetails> UpdateDetailsForNewInstance(string studyInstanceUID, decimal totalSizeFile);

        /// <summary>
        /// Retrieves the most recent studies for a specified institution.
        /// </summary>
        /// <param name="parameters">The pagination parameters</param>
        /// <returns>
        /// A task result containing a list of StudyDto objects representing the recent studies,
        /// paginated according to the provided parameters.
        /// </returns>
        Task<List<StudyDto>> AllStudiesByControlParams (ControlQueryParametersDto parameters);

        ///// <summary>
        ///// Maps the provided metadata from DICOM to a StudyCreateDto object.
        ///// </summary>
        ///// <param name="metadata">The metadata containing the main entities required for creating a Study entity.</param>
        ///// <returns>
        ///// A StudyCreateDto object mapped from the provided metadata, ready to be used for creating a new Study entity.
        ///// </returns>
        //Task<StudyCreateDto> MapToCreateDto(MainEntitiesCreateDto metadata);

        /// <summary>
        /// Maps the provided DICOM metadata to an InstanceCreateDto object.
        /// </summary>
        /// <param name="metadata">The metadata containing the main entities and DICOM information required to create an Instance.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an InstanceCreateDto object
        /// mapped from the provided metadata, ready to be used for creating a new Instance entity.
        /// </returns>
        Task<StudyCreateDto> MapMetadataToCreateDto(MetadataDto metadata);
    }
}
