using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Models.Dto.DicomWeb.Stow;
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

        ///////** OLD VERSION , BECOMES PRIVATE
        ///// <summary>
        ///// Registers the main entities in the PACS system, such as Study, Series, and Instance.
        ///// </summary>
        ///// <param name="RequestDto">The DTO containing the details of the main entities to be registered.</param>
        ///// <returns>A task that represents the asynchronous operation. The task result contains a string identifier of the registered entities.</returns> 
        //Task<List<StowInstanceResult>> RegisterDicomInstances(List<DicomFilePackage> dicomFilePackages, string referencedStudyUID = null);


        //STORE-RS

        // NUEVO METODO
        Task<string> ProcessStowRsRequest(HttpRequest httpRequest, string referencedStudyUID);


        //QIDO-RS

        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable list of StudyDto objects.</returns>
        /// <summary>
        /// Retrieves a list of all studies (QIDO-RS).
        /// </summary>
        /// <param name="queryParams">
        /// Optional parameters for the query, such as filters and other search criteria.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation. The task contains a string in JSON format with the list of studies.
        /// </returns>
        Task<string> GetAllStudies(IQueryCollection queryParams);

        /// <summary>
        /// Retrieves a list of all series associated with a specific study (QIDO-RS).
        /// </summary>
        /// <param name="studyInstanceUID">
        /// Unique identifier of the study (StudyInstanceUID) from which the series are to be retrieved.
        /// </param>
        /// <param name="queryParams">
        /// Optional parameters for the query, such as filters and other search criteria.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation. The task contains a string in JSON format with the list of series for the specified study.
        /// </returns>
        Task<string> GetAllSeriesFromStudy(string studyInstanceUID, IQueryCollection queryParams);

        /// <summary>
        /// Retrieves a list of all instances associated with a specific series of a study (QIDO-RS).
        /// </summary>
        /// <param name="studyInstanceUID">
        /// Unique identifier of the study (StudyInstanceUID) that contains the desired series.
        /// </param>
        /// <param name="seriesInstanceUID">
        /// Unique identifier of the series (SeriesInstanceUID) from which the instances are to be retrieved.
        /// </param>
        /// <param name="queryParams">
        /// Optional parameters for the query, such as filters and other search criteria.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation. The task contains a string in JSON format with the list of instances for the specified series.
        /// </returns>
        Task<string> GetAllInstancesFromSeries(string studyInstanceUID, string seriesInstanceUID, IQueryCollection queryParams);

    }
}
