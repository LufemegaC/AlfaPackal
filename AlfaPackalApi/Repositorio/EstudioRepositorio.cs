using AlfaPackalApi.Datos;
using AlfaPackalApi.Modelos;
using AlfaPackalApi.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace AlfaPackalApi.Repositorio
{
    public class EstudioRepositorio : Repositorio<Estudio>, IEstudioRespositorio
    {
        private readonly ApplicationDbContext _db;
        public EstudioRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Estudio> Actualizar(Estudio entidad)
        {
            //entidad.FechaActualizacion = DateTime.Now; 
            _db.Estudios.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
        public async Task<bool> ExisteStudyInstanceUID(string studyInstanceUID)
        {
            return await _db.Estudios.AnyAsync(e => e.StudyInstanceUID == studyInstanceUID);
        }

    }
}
