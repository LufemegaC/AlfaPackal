using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Models.Dto;

namespace Api_PACsServer.Orchestrators.IOrchestrator
{
    public interface IDicomOrchestrator
    {
        /// <summary>
        /// Registers the main entities in the PACS system, such as Study, Series, and Instance.
        /// </summary>
        /// <param name="mainEntitiesCreate">The DTO containing the details of the main entities to be registered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a string identifier of the registered entities.</returns> 
        Task<string> RegisterMainEntities(MainEntitiesCreateDto mainEntitiesCreate);

        
    }
}
