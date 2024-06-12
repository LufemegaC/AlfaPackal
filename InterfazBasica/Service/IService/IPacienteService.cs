using InterfazBasica.Models.Pacs;
using Utileria;

namespace InterfazBasica_DCStore.Service.IService
{
    public interface IPacienteService
    {
        Task<T> GetAll<T>(string token);
        Task<T> GetByPACS_ID<T>(int pacsId,string token);
        Task<T> GetByGeneratedPatientID<T>(string generatedId,string token);
        Task<T> GetByName<T>(string name,string token);
        Task<T> ExistByMetadata<T>(string patientID, string issuerOfPatientID,string token);
        Task<T> Create<T>(PacienteCreateDto dto,string token);
        //
        //Task<T> CreatePruebas<T>(PacienteCreateDto dto);
    }
}
