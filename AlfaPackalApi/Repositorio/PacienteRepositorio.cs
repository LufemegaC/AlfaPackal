using AlfaPackalApi.Datos;
using AlfaPackalApi.Modelos;
using AlfaPackalApi.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AlfaPackalApi.Repositorio
{
    public class PacienteRepositorio : Repositorio<Paciente>, IPacienteRepositorio
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
