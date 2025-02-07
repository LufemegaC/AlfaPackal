using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto.DicomWeb;
using Api_PACsServer.Models.Dto.DicomWeb.Qido;
using Api_PACsServer.Models.Dto.Series;
using Api_PACsServer.Models.Dto.Studies;
using Api_PACsServer.Models.OHIFVisor;
using Api_PACsServer.Models.Supplement;

namespace Api_PACsServer.Services.IService.MainEntities
{
    /// <summary>
    /// Service interface for handling Series-related operations in the PACS system.
    /// </summary>
    public interface ISerieService
    {
        /// <summary>
        /// Creates a new Series entity.
        /// </summary>
        /// <param name="serieDto">The data transfer object containing the details of the Series to be created.</param>
        /// <returns>The created Series entity.</returns>
        Task<Serie> Create(SerieCreateDto serieDto);

        ///// <summary>
        ///// Retrieves a Series entity by its Study ID and Series Number.
        ///// </summary>
        ///// <param name="studyId">The ID of the Study associated with the Series.</param>
        ///// <param name="seriesNumber">The unique number of the Series within the Study to be retrieved.</param>
        ///// <returns>
        ///// A task that represents the asynchronous operation. The task result contains the Series entity 
        ///// that matches the provided Study ID and Series Number.
        ///// </returns>
        //Task<Serie> GetById(int studyId, int seriesNumber);

        /// <summary>
        /// Gets a Series entity by its SerieInstanceUID.
        /// </summary>
        /// <param name="serieInstanceUID">The UID of the Series to be retrieved.</param>
        /// <returns>the Series entity.</returns>
        Task<Serie> GetByUID(string serieInstanceUID);

        /// <summary>
        /// Checks if a Series entity exists by its UID.
        /// </summary>
        /// <param name="serieInstanceUID">The UID of the Series to be checked.</param>
        /// <returns>The task result contains a boolean indicating whether the Series exists.</returns>
        Task<bool> ExistsByUID(string serieInstanceUID);


        /// <summary>
        /// Gets all Series entities associated with a given Study ID.
        /// </summary>
        /// <param name="studyInstanceUID">The UID of the Study whose Series are to be retrieved.</param>
        /// <returns>he task result contains a collection of Series DTOs.</returns>
        Task<IEnumerable<OHIFSerie>> GetAllByStudyUID(string studyInstanceUID);

        /// <summary>
        /// Updates the load information of a Series entity for a new instance.
        /// </summary>
        /// <param name="seriesInstanceUID">The UID of the study.</param>
        /// <param name="totalSizeFile">The total size of the file in MB.</param>
        /// <returns>The task result contains the updated Series entity.</returns>
        Task<SerieDetails> UpdateDetailsForNewInstance(string seriesInstanceUID, decimal totalSizeFile);

        /// <summary>
        /// Retrieves a list of series based on the specified query parameters  and Study UID.
        /// </summary>
        /// <param name="requestParameters">
        /// An object containing the query parameters for filtering and controlling the study retrieval process.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of <see cref="StudyDto"/> 
        /// objects that match the specified query criteria.
        /// </returns>
        Task<List<SerieDto>> GetSeriesFromStudy(QueryRequestParameters<SerieQueryParametersDto> requestParameters);

        /// <summary>
        /// Maps the provided DICOM metadata to an InstanceCreateDto object.
        /// </summary>
        /// <param name="metadata">The metadata containing the main entities and DICOM information required to create an Instance.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an InstanceCreateDto object
        /// mapped from the provided metadata, ready to be used for creating a new Instance entity.
        /// </returns>
        Task<SerieCreateDto> MapMetadataToCreateDto(MetadataDto metadata);
    }
}
