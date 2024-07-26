using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Utileria.DicomValues;

namespace AlfaPackalApi.Modelos.Dto.Pacs
{
    public class PacienteDto
    {
        // ID Paciente (Metadato)
        public int PACS_PatientID { get; set; } // ID interno/sistema
        public string? PatientID { get; set; } // ID del paciente DICOM/Metadato
        public string GeneratedPatientID { get; set; }
        // Nombre completo de paciente         
        public string? PatientName { get; set; }
		// Edad de paciente        
        public string? PatientAge { get; set; }
        // Sexo de paciente        
        public string? PatientSex { get; set; }
        //Peso del paciente        
        public string? PatientWeight { get; set; }
        //Fecha de nacimiento
        public DateTime? PatientBirthDate { get; set; } // Hacer nullable para manejar casos sin esta información
        // Campos adicionales sugeridos por la estructura DICOM        
        public string? IssuerOfPatientID { get; set; } //el emisor del PatientID,
    }
}
