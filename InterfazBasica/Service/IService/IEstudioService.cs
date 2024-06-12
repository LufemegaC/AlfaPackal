using InterfazBasica.Models.Pacs;

namespace InterfazBasica.Service.IService
{
    public interface IEstudioService
    {
        Task<T> GetAll<T>(string token);
        Task<T> GetByID<T>(int id,string token);
        Task<T> GetAllByDate<T>(DateTime date,string token);
        Task<T> GetStudyByInstanceUID<T>(string instanceUID,string token);
        Task<T> Create<T>(EstudioCreateDto dto,string token);
        Task<T> ExistStudyByInstanceUID<T>(string instanceUID,string token);
        Task<T> GetStudyByAccessionNumber<T>(string accessionNumber,string token);
    }
}
