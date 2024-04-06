using AlfaPackalApi.Datos;
using AlfaPackalApi.Modelos;
using AlfaPackalApi.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace AlfaPackalApi.Repositorio
{
    public class EstudioRepositorio : Repositorio<Estudio>, IEstudioRepositorio
    {
        private readonly ApplicationDbContext _db;
        public EstudioRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Estudio> GetByAccessionNumber(string accessionNumbers)
        {
            return await _db.Estudios.FirstOrDefaultAsync(e => e.AccessionNumber == accessionNumbers);
        }

        public async Task <Estudio> GetByInstanceUID(string studyInstanceUID)
        {
            return await _db.Estudios.FirstOrDefaultAsync(e => e.StudyInstanceUID == studyInstanceUID);
        }

        public async Task<bool> ExistByInstanceUID(string studyInstanceUID)
        {
            return await _db.Estudios.AnyAsync(e => e.StudyInstanceUID == studyInstanceUID);
        }
    }
}
