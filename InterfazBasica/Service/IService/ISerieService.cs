using InterfazBasica.Models.Pacs;
using System.Threading.Tasks;

namespace InterfazBasica_DCStore.Service.IService
{
    public interface ISerieService
    {
        Task<T> GetAll<T>(string token);
        Task<T> GetByID<T>(int id,string token);
        //Task<T> GetAllByStudyInstanceUID<T>(string uid,string token);
        Task<T> GetBySeriesInstanceUID<T>(string instanceUID,string token);
        Task<T> Create<T>(SerieCreateDto dto,string token);
        Task<T> ExistByInstanceUID<T>(string instanceUID,string token);
    }
}
