using AlfaPackalApi.Datos;
using AlfaPackalApi.Modelos;
using AlfaPackalApi.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace AlfaPackalApi.Repositorio
{
    public class DoctorRepositorio : Repositorio<Doctor>, IDoctorRepositorio
    {
        private readonly ApplicationDbContext _db;
        public DoctorRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Doctor> Actualizar(Doctor entidad)
        {
            //entidad.FechaActualizacion = DateTime.Now; 
            _db.Doctores.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
        public async Task<bool> ExisteNombre(string nombre)
        {
            return await _db.Doctores.AnyAsync(e => e.Nombre == nombre);
        }

    }
}
