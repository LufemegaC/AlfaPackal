using InterfazBasica.Models.Pacs;
using System.Threading.Tasks;

namespace InterfazBasica_DCStore.Service.IService
{
    public interface ISerieService
    {
        Task<T> GetAll<T>();
        Task<T> GetByID<T>(int id);
        //Task<T> GetAllByStudyInstanceUID<T>(string uid);
        Task<T> GetBySeriesInstanceUID<T>(string instanceUID);
        Task<T> Create<T>(SerieCreateDto dto);
        Task<T> ExistByInstanceUID<T>(string instanceUID);
    }
}
