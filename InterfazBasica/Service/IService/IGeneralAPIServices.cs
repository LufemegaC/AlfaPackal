using InterfazBasica_DCStore.Models;

namespace InterfazBasica_DCStore.Service.IService
{
    public interface IGeneralAPIServices
    {
        Task<T> ValidateEntities<T>(MainEntitiesValues mainEntitiesValues,string token);
        // Obtiene informacion de listado principal
        Task<T> GetMainList<T>(string token);

    }
}
