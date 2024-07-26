using AlfaPackalApi.Datos;
using AlfaPackalApi.Modelos;
using Api_PACsServer.Repositorio;
using Api_PACsServer.Repositorio.IRepositorio.Pacs;
using Microsoft.EntityFrameworkCore;

namespace Api_PACsServer.Repositorio.Pacs
{
    public class PacienteRepositorio : RepositorioLecturaYEscritura<Paciente>, IPacienteRepositorio
    {
        private readonly ApplicationDbContext _db;
        public PacienteRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Paciente> GetByName(string nombre)
        {
            return await _db.Pacientes.FirstOrDefaultAsync(e => e.PatientName.ToLower().Contains(nombre.ToLower()));
        }


        public async Task<Paciente> GetByGeneratedPatientId(string generatedId)
        {
            return await _db.Pacientes.FirstOrDefaultAsync(e => e.GeneratedPatientID == generatedId);
        }

        public async Task<Paciente> GetByMetadata(string patientID, string issuerOfPatientId)
        {
            return await _db.Pacientes.FirstOrDefaultAsync(e => e.PatientID == patientID && e.IssuerOfPatientID == issuerOfPatientId);
        }
        public async Task<bool> ExistByMetadata(string patienteID, string issuerOfPatientId)
        {
            return await _db.Pacientes.AnyAsync(e => e.PatientID == patienteID &&
                                                e.IssuerOfPatientID == issuerOfPatientId);
        }

        //Metodo para proceso C-FIND
        public async Task<List<string>> FindByCriteria(string patientName, string patientId)
        {
            var query = _db.Pacientes.AsQueryable();
            if (!string.IsNullOrEmpty(patientName))
            {
                query = query.Where(p => p.PatientName.Contains(patientName));
            }

            if (!string.IsNullOrEmpty(patientId))
            {
                query = query.Where(p => p.PatientID.Contains(patientId));
            }
            // Devuelve solo la lista de PACS_PatientID
            return await query.Select(p => p.PACS_PatientID.ToString()).ToListAsync();
        }
    }
}
