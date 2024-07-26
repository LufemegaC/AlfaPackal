using AlfaPackalApi.Modelos.Dto.Pacs;
using Api_PACsServer.Modelos.Dto;

namespace Api_PACsServer.Services.IService.Pacs
{
    public interface ISerieService
    {
        Task<SerieDto> Create(SerieCreateDto serieDto);

        Task<SerieDto> GetById(int id);

        Task<SerieDto> GetByUID(string InstanceUID);

        Task<IEnumerable<SerieDto>> GetAllByStudyPacsId(int studyPacsId);

        Task<IEnumerable<SerieDto>> GetAllByStudyUID(string studyUID);
    }
}
