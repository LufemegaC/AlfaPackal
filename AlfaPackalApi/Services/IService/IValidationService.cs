using Api_PACsServer.Modelos;

namespace Api_PACsServer.Services.IService
{
    public interface IValidationService
    {
        Task<MainEntitiesValues> ValidarExistenciaDeEntidades(MainEntitiesValues mainEntitiesValues);
    }
}
