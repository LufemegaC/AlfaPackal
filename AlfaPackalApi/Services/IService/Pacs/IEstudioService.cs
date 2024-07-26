using AlfaPackalApi.Modelos;
using AlfaPackalApi.Modelos.Dto.Pacs;


namespace Api_PACsServer.Services.IService.Pacs
{
    public interface IEstudioService
    {
        Task<EstudioDto> Crear(EstudioCreateDto createDto);

        Task<EstudioDto> GetById(int id);

        Task<EstudioDto> GetByUID(string InstanceUID);
        EstudioDto MapToDto(Estudio estudio);

        //Task<IEnumerable<EstudioDto>> GetAllBySeriePacsId(int pacsSerieId);

        //Task<IEnumerable<EstudioDto>> GetAllBySerieUID(string SerieUID);


    }
}
