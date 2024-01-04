using InterfazBasica.Models.Pacs;

namespace InterfazBasica.Service.IService
{
    public interface IEstudioService
    {
        Task<T> ObtenerTodos<T>();
        Task<T> Obtener<T>(int id);
        Task<T> Crear<T>(EstudioCreateDto dto);
        Task<T> Actualizar<T>(EstudioUpdateDto dto);
        Task<T> Remover<T>(int id);
    }
}
