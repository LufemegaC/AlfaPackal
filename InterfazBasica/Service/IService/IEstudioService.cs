using InterfazBasica.Models.Pacs;

namespace InterfazBasica.Service.IService
{
    public interface IEstudioService
    {
        Task<T> GetAll<T>();
        Task<T> GetByID<T>(int id);
        Task<T> GetAllByDate<T>(DateTime date);
        Task<T> GetStudyByInstanceUID<T>(string instanceUID);
        Task<T> Create<T>(EstudioCreateDto dto);
        Task<T> ExistStudyByInstanceUID<T>(string instanceUID);
        Task<T> GetStudyByAccessionNumber<T>(string accessionNumber);
    }
}
