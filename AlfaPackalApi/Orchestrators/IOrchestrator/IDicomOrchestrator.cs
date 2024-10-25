using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Models.Dto.DicomWeb;
using Api_PACsServer.Models.Dto.Studies;

namespace Api_PACsServer.Orchestrators.IOrchestrator
{
    public interface IDicomOrchestrator
    {
        /////** PRIMERA VERSION
        ///// <summary>
        ///// Registers the main entities in the PACS system, such as Study, Series, and Instance.
        ///// </summary>
        ///// <param name="mainEntitiesCreate">The DTO containing the details of the main entities to be registered.</param>
        ///// <returns>A task that represents the asynchronous operation. The task result contains a string identifier of the registered entities.</returns> 
        //Task<OperationResult> RegisterMainEntities(MainEntitiesCreateDto mainEntitiesCreate);

        /////** NUEVA VERSION PARA DICOMWEB
        /// <summary>
        /// Registers the main entities in the PACS system, such as Study, Series, and Instance.
        /// </summary>
        /// <param name="RequestDto">The DTO containing the details of the main entities to be registered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a string identifier of the registered entities.</returns> 
        Task<List<DicomOperationResult>> RegisterDicomInstances(List<DicomFilePackage> dicomFilePackages, string referencedStudyUID = null);

        /// <summary>
        /// Retrieves the most recent studies for a specified institution, with pagination.
        /// </summary>
        
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable list of StudyDto objects.</returns>
        Task<List<StudyDto>> GetInfoStudy(StudyQueryParametersDto studyQuery, ControlQueryParametersDto controlQuery);
    }
}
