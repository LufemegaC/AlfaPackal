using InterfazBasica.Models.Pacs;
using Utileria;

namespace InterfazBasica_DCStore.Service.IService
{
    public interface IPacienteService
    {
        Task<T> GetAll<T>();
        Task<T> GetByPACS_ID<T>(int pacsId);
        Task<T> GetByGeneratedPatientID<T>(string generatedId);
        Task<T> GetByName<T>(string name);
        Task<T> ExistByMetadata<T>(string patientID, string issuerOfPatientID);
        Task<T> Create<T>(PacienteCreateDto dto);
        //
        //Task<T> CreatePruebas<T>(PacienteCreateDto dto);
    }
}
