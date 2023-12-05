using AlfaPackalApi.Datos;
using AlfaPackalApi.Modelos;
using AlfaPackalApi.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace AlfaPackalApi.Repositorio
{
    public class PacienteRepositorio : Repositorio<Paciente>, IPacienteRepositorio
    {
        private readonly ApplicationDbContext _db;
        public PacienteRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Paciente> Actualizar(Paciente entidad)
        {
            //entidad.FechaActualizacion = DateTime.Now; 
            _db.Pacientes.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
        public async Task<bool> ExisteNombre(string nombre)
        {
            return await _db.Pacientes.AnyAsync(e => e.Nombre == nombre);
        }

    }
}
