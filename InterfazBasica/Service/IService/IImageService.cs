using InterfazBasica.Models.Pacs;

namespace InterfazBasica_DCStore.Service.IService
{
    public interface IImageService
    {
        Task<T> GetAll<T>();
        Task<T> GetbyID<T>(int id);
        Task<T> GetbySOPInstanceUID<T>(string uid);
        Task<T> Create<T>(ImagenCreateDto dto);
        //Task<T> GetAllByStudyInstanceUID<T>(string uid);
        Task<T> ExistBySOPInstanceUID<T>(string sopInstanceUID);

    }
}
