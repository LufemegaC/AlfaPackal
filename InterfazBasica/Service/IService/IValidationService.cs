using InterfazBasica_DCStore.Models;

namespace InterfazBasica_DCStore.Service.IService
{
    public interface IValidationService
    {
        Task<T> ValidateEntities<T>(MainEntitiesValues mainEntitiesValues);

    }
}
