using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Load;
using Api_PACsServer.Models.Dto.Series;

namespace Api_PACsServer.Services.IService.Pacs
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

        /// <summary>
        /// Gets a Series entity by its ID.
        /// </summary>
        /// <param name="serieId">The ID of the Series to be retrieved.</param>
        /// <returns>the Series entity.</returns>
        Task<Serie> GetById(int serieId);

        /// <summary>
        /// Gets a Series entity by its SerieInstanceUID.
        /// </summary>
        /// <param name="serieInstanceUID">The UID of the Series to be retrieved.</param>
        /// <returns>the Series entity.</returns>
        Task<Serie> GetByUID(string serieInstacenUID);

        /// <summary>
        /// Checks if a Series entity exists by its UID.
        /// </summary>
        /// <param name="serieInstanceUID">The UID of the Series to be checked.</param>
        /// <returns>The task result contains a boolean indicating whether the Series exists.</returns>
        Task<bool> ExistsByUID(string serieInstacenUID);


        /// <summary>
        /// Gets all Series entities associated with a given Study ID.
        /// </summary>
        /// <param name="studyId">The ID of the Study whose Series are to be retrieved.</param>
        /// <returns>he task result contains a collection of Series DTOs.</returns>
        Task<IEnumerable<SerieDto>> GetAllByStudyPacsId(int studyId);

        ///// <summary>
        ///// Gets all Series entities associated with a given Study UID.
        ///// </summary>
        ///// <param name="studyInstanceUID">The UID of the Study whose Series are to be retrieved.</param>
        ///// <returns>The task result contains a collection of Series DTOs.</returns>
        //Task<IEnumerable<SerieDto>> GetAllByStudyUID(string studyInstanceUID);

        /// <summary>
        /// Updates the load information of a Series entity for a new instance.
        /// </summary>
        /// <param name="serieId">The ID of the Series to be updated.</param>
        /// <param name="totalSizeFile">The total size of the file in MB.</param>
        /// <returns>The task result contains the updated Series entity.</returns>
        Task<SerieLoad> UpdateLoadForNewInstance(int serieId, decimal totalSizeFile);
    }
}
