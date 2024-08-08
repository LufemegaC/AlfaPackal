using Api_PACsServer.Modelos;
using Api_PACsServer.Models.Dto.Instances;

namespace Api_PACsServer.Services.IService.Pacs
{
    /// <summary>
    /// Service interface for handling Instance-related operations in the PACS system.
    /// </summary>
    public interface IInstanceService
    {
        
        /// <summary>
        /// Creates a new Instance entity.
        /// </summary>
        /// <param name="createDto">The data transfer object containing the details of the Instance to be created.</param>
        /// <returns>the created Instance entity.</returns>
        Task<Instance> Create(InstanceCreateDto createDto);

        /// <summary>
        /// Gets an Instance entity by its ID (internal).
        /// </summary>
        /// <param name="instanceId">The system ID of the Instance.</param>
        /// <returns>the Instance entity.</returns>
        Task<Instance> GetById(int instanceId);

        /// <summary>
        /// Gets an Instance entity by its SOPInstanceUID.
        /// </summary>
        /// <param name="SOPInstanceUID">The UID of the Instance to be retrieved.</param>
        /// <returns>the Instance entity.</returns>
        Task<Instance> GetByUID(string SOPInstanceUID);

        /// <summary>
        /// Checks if an Instance entity exists by its SOPInstanceUID.
        /// </summary>
        /// <param name="SOPInstanceUID">The UID of the Instance to be checked.</param>
        /// <returns>a boolean indicating whether the Instance exists.</returns>
        Task<bool> ExistsByUID(string SOPInstanceUID);

        /// <summary>
        /// Gets all Instance entities associated with a given SOPInstanceUID.
        /// </summary>
        /// <param name="serieId">The system I of the Series whose Instances are to be retrieved.</param>
        /// <returns>a collection of Instance DTOs.</returns>
        Task<IEnumerable<InstanceDto>> GetAllBySerieId(int serieId);

        ///// <summary>
        ///// Gets all Instance entities associated with a given SerieInstanceUID.
        ///// </summary>
        ///// <param name="serieInstanceUID">The UID of the Series whose Instances are to be retrieved.</param>
        ///// <returns>a collection of Instance DTOs.</returns>
        //Task<IEnumerable<InstanceDto>> GetAllBySerieUID(string serieInstanceUID);
    }
}
