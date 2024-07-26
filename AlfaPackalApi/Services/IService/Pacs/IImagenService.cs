using AlfaPackalApi.Modelos.Dto.Pacs;
using Api_PACsServer.Modelos.Dto;

namespace Api_PACsServer.Services.IService.Pacs
{
    public interface IImagenService
    {
        Task<ImagenDto> Create(ImagenCreateDto imagen);

        Task<ImagenDto> GetById(int id);

        Task<ImagenDto> GetByUID(string InstanceUID);

        Task<IEnumerable<ImagenDto>> GetAllBySerieId(int pacsSerieId);

        Task<IEnumerable<ImagenDto>> GetAllBySerieUID(string SerieUID);

    }
}
