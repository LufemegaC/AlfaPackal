using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Dto.Vistas;
using Api_PACsServer.Modelos.Especificaciones;

namespace Api_PACsServer.Services.IService
{
    public interface IGeneralAPIService
    {
        Task<MainEntitiesValues> ValidarExistenciaDeEntidades(MainEntitiesValues mainEntitiesValues);

        // Listado principal
        //Task<List<EstudioConPacienteDto>> ObtenerEstudiosConPaciente();

        //// Listado principal
        //Task<List<StudyListDto>> GetStudyList(int institutionId);
        //// Listado principal

        //PagedList<StudyListDto> ListaEstudiosPaginado(ParametrosPag parametros);

    }
}
