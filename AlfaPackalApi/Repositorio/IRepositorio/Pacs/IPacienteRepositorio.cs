using AlfaPackalApi.Modelos;

namespace Api_PACsServer.Repositorio.IRepositorio.Pacs
{
    public interface IPacienteRepositorio : IRepositorioEscritura<Paciente>, IRepositorioLectura<Paciente>
    {
        //Task<Paciente> Actualizar(Paciente entidad);
        Task<Paciente> GetByName(string nombre);
        Task<Paciente> GetByGeneratedPatientId(string generatedId);
        Task<Paciente> GetByMetadata(string patientID, string issuerOfPatientId);
        Task<bool> ExistByMetadata(string patientID, string issuerOfPatientId);
        // Metodo para C-FIND
        Task<List<string>> FindByCriteria(string patientName, string patientId);
    }
}
