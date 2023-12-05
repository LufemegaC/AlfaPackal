using AlfaPackalApi.Modelos;

namespace AlfaPackalApi.Repositorio.IRepositorio
{
    public interface IDoctorRepositorio : IRepositorio<Doctor>
    {
        Task<Doctor> Actualizar(Doctor entidad);
        Task<bool> ExisteNombre(string nombre);
    }
}
