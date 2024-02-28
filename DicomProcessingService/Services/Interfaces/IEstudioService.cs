using DicomProcessingService.Dtos;

namespace DicomProcessingService.Services.Interfaces
{
    public interface IEstudioService
    {
        Task<T> ObtenerTodos<T>();
        Task<T> Obtener<T>(int id);
        Task<T> Crear<T>(EstudioCreateDto dto);
        Task<T> Remover<T>(int id);
    }
}
