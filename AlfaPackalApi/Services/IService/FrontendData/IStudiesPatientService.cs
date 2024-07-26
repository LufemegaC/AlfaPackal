using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Dto.Vistas;
using Api_PACsServer.Modelos.Especificaciones;

namespace Api_PACsServer.Services.IService.FrontendData
{
    public interface IStudiesPatientService
    {
        PagedList<StudyListDto> GetMainStudiesList(ParametrosPag parametros);

        Task<MainEntitiesValues> ValidarExistenciaDeEntidades(MainEntitiesValues mainEntitiesValues);
    }
}
