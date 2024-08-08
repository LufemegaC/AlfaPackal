using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Orchestrators.IOrchestrator;
using Api_PACsServer.Services.IService.Pacs;

namespace Api_PACsServer.Orchestrators
{
    public class UserOchestrator: IUserOchestrator
    {
        private readonly IStudyService _studyService;
        public UserOchestrator(IStudyService studyService)
        {
            _studyService = studyService;
        }
        public UserStudiesListDto GetRecentStudies(PaginationParameters parameters)
        {
            return _studyService.GetRecentStudies(parameters);
        }
    }
}
