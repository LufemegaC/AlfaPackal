using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto.Base;
using Api_PACsServer.Models.Dto.DicomWeb;
using Api_PACsServer.Models.Dto.DicomWeb.Qido;
using Api_PACsServer.Models.Dto.Studies;
using Api_PACsServer.Models.OHIFVisor;
using Api_PACsServer.Models.Supplement;

namespace Api_PACsServer.Services.IService.MainEntities
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
        Task UpdateForNewSerie(string studyInstanceUID, string modality);

        /// <summary>
        /// Updates the load information of a Study entity for a new instance.
        /// </summary>
        /// <param name="studyInstanceUID">The UID of the Study to be updated.</param>
        /// <param name="totalSizeFile">The total size of the file in MB.</param>
        /// <returns>The task result contains the updated StudyLoad entity.</returns>
        Task<StudyDetails> UpdateDetailsForNewInstance(string studyInstanceUID, decimal totalSizeFile);

        /// <summary>
        /// Retrieves a list of studies based on the specified query parameters.
        /// </summary>
        /// <param name="requestParameters">
        /// An object containing the query parameters for filtering and controlling the study retrieval process.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of <see cref="StudyDto"/> 
        /// objects that match the specified query criteria.
        /// </returns>
        Task<List<StudyDto>> GetStudyData(QueryRequestParameters<StudyQueryParametersDto> requestParameters);


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
