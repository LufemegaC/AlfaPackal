using AlfaPackalApi.Datos;
using AlfaPackalApi.Modelos;
using AlfaPackalApi.Repositorio.IRepositorio;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Api_PACsServer.Modelos.Dto.Vistas;

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

        public async Task<Estudio> GetByInstanceUID(string studyInstanceUID)
        {
            return await _db.Estudios.FirstOrDefaultAsync(e => e.StudyInstanceUID == studyInstanceUID);
        }

        public async Task<bool> ExistByInstanceUID(string studyInstanceUID)
        {
            return await _db.Estudios.AnyAsync(e => e.StudyInstanceUID == studyInstanceUID);
        }

        //Metodo para proceso C-FIND
        public async Task<List<Estudio>> FindByPatientIds(List<string> pacsPatientIds)
        {
            return await _db.Estudios
                    .Where(e => pacsPatientIds.Contains(e.PACS_PatientID.ToString()))
                    .Take(50) // Limita a los primeros 50 registros
                    .ToListAsync();
        }

        
    }
}
