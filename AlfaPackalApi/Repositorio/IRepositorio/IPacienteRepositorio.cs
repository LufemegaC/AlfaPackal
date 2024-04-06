using AlfaPackalApi.Modelos;

namespace AlfaPackalApi.Repositorio.IRepositorio
{
    public interface IPacienteRepositorio : IRepositorio<Paciente>
    {
        //Task<Paciente> Actualizar(Paciente entidad);
        Task<Paciente> GetByName(string nombre);
        Task<Paciente> GetByGeneratedPatientId(string generatedId);
        Task<Paciente> GetByMetadata(string patientID, string issuerOfPatientId);
        Task<bool> ExistByMetadata(string patientID,string issuerOfPatientId);
    }
}
