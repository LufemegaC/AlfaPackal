using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Orchestrators.IOrchestrator;
using Api_PACsServer.Services.IService.Pacs;

namespace Api_PACsServer.Orchestrators
{
    public class DicomOrchestrator : IDicomOrchestrator
    {
        private readonly IStudyService _studyService;
        private readonly ISerieService _serieService;
        private readonly IInstanceService _instanceService;

        public DicomOrchestrator(IStudyService studyService, ISerieService serieService,
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
            var fileSize = mainEntitiesCreate.TotalFileSizeMB;
            // main UIDs
            var studyInstanceUID = mainEntitiesCreate.StudyInstanceUID;
            var serieInstanceUID = mainEntitiesCreate.SeriesInstanceUID;
            var sopInstanceUID = mainEntitiesCreate.SOPInstanceUID;

            // Check if entities exists
            bool studyExists = await _studyService.ExistsByUID(studyInstanceUID);
            bool seriesExists = await _serieService.ExistsByUID(serieInstanceUID);
            bool instanceExists = await _instanceService.ExistsByUID(sopInstanceUID);
            //bool EntitiesExists = (studyExists) && (seriesExists) && (instanceExists);

            // Mapping create entitities
            var studyCreateDto = await _studyService.MapToCreateDto(mainEntitiesCreate);
            var serieCreateDto = await _serieService.MapToCreateDto(mainEntitiesCreate);
            var instanceCreateDto = await _instanceService.MapToCreateDto(mainEntitiesCreate);

            if(!instanceExists)
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
                    if (studyExists)
                    {
                        await _studyService.UpdateDetailsForNewSerie(study.StudyInstanceUID);
                    }
                }
                else
                {
                    serie = await _serieService.GetByUID(serieInstanceUID);
                }
                // If the Instance does not exist, create it
                Instance instance = await _instanceService.Create(instanceCreateDto);
                if (studyExists)
                {
                    await _studyService.UpdateDetailsForNewInstance(study.StudyInstanceUID, fileSize);
                }
                if (seriesExists)
                {
                    await _serieService.UpdateLoadForNewInstance(serie.SeriesInstanceUID, fileSize);
                }
                return "Entities registered successfully.";
            }
            {
                return "Entities already Exists.";
            } 
        }
    }
}
