using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Models.Dto.Studies;
using Api_PACsServer.Orchestrators.IOrchestrator;
using Api_PACsServer.Services;
using Api_PACsServer.Services.IService.Pacs;

namespace Api_PACsServer.Orchestrators
{
    public class DicomOrchestrator : IDicomOrchestrator
    {
        private readonly IStudyService _studyService;
        private readonly ISerieService _serieService;
        private readonly IInstanceService _instanceService;

        public DicomOrchestrator(IStudyService studyService, SerieService serieService,
                                    IInstanceService instanceService)
        {
            _studyService = studyService;
            _serieService = serieService;
            _instanceService = instanceService;
        }

        /// <remarks>
        /// Pendiente :
        /// 1.- Cambiar el retorno de las main por solo el PACSId ( cancelado )
        /// </remarks>
        public async Task<string> RegisterMainEntities(MainEntitiesCreateDto mainEntitiesCreate)
        {
            //file size
            var fileSize = mainEntitiesCreate.StudyCreate.TotalFileSizeMB;
            // main UIDs
            var studyInstanceUID = mainEntitiesCreate.StudyCreate.StudyInstanceUID;
            var serieInstanceUID = mainEntitiesCreate.SerieCreate.SeriesInstanceUID;
            var sopInstanceUID = mainEntitiesCreate.InstanceCreate.SOPInstanceUID;

            // Check if entities exists
            bool studyExists = await _studyService.ExistsByUID(studyInstanceUID);
            bool seriesExists = await _serieService.ExistsByUID(serieInstanceUID);
            bool instanceExists = await _instanceService.ExistsByUID(sopInstanceUID);

            // If the Study does not exist, create it
            Study study;
            if (!studyExists)
            {
                study = await _studyService.Create(mainEntitiesCreate.StudyCreate);
            }
            else
            {
                study = await _studyService.GetByUID(studyInstanceUID);
            }
            // If the Series does not exist, create it
            Serie serie;
            if (!seriesExists)
            {
                mainEntitiesCreate.SerieCreate.PACSStudyID = study.PACSStudyID;
                serie = await _serieService.Create(mainEntitiesCreate.SerieCreate);
                if (studyExists)
                {
                    await _studyService.UpdateLoadForNewSerie(study.PACSStudyID);
                }
            }
            else
            {
                serie = await _serieService.GetByUID(serieInstanceUID);
            }
            // If the Instance does not exist, create it
            Instance instance;
            if (!instanceExists)
            {
                mainEntitiesCreate.InstanceCreate.PACSSerieID = serie.PACSSerieID;
                instance = await _instanceService.Create(mainEntitiesCreate.InstanceCreate);
                if (studyExists)
                {
                    await _studyService.UpdateLoadForNewInstance(study.PACSStudyID, fileSize);
                }
                if (seriesExists)
                {
                    await _serieService.UpdateLoadForNewInstance(serie.PACSSerieID, fileSize);
                }
            }
            return "Entities registered successfully.";
        }
    }
}
