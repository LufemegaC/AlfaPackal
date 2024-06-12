using InterfazBasica.Models.Pacs;

namespace InterfazBasica_DCStore.Service.IService
{
    public interface IImageService
    {
        Task<T> GetAll<T>(string token);
        Task<T> GetbyID<T>(int id,string token);
        Task<T> GetbySOPInstanceUID<T>(string uid,string token);
        Task<T> Create<T>(ImagenCreateDto dto,string token);
        //Task<T> GetAllByStudyInstanceUID<T>(string uid,string token);
        Task<T> ExistBySOPInstanceUID<T>(string sopInstanceUID,string token);

    }
}
