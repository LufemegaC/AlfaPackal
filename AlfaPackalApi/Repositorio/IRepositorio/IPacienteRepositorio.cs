using AlfaPackalApi.Modelos;

namespace AlfaPackalApi.Repositorio.IRepositorio
{
    public interface IPacienteRepositorio : IRepositorio<Paciente>
    {
        Task<Paciente> Actualizar(Paciente entidad);
        Task<bool> ExisteNombre(string nombre);
    }
}
