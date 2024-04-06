using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Utileria.DicomValues;

namespace InterfazBasica.Models.Pacs
{
    public class PacienteCreateDto
    {
        // ID Paciente (Metadato) 
        [StringLength(64)]        
        public string? PatientID { get; set; } // ID del paciente DICOM/Metadato
        // Nombre completo de paciente 
        [Required, StringLength(64)]
        public string GeneratedPatientID { get; set; }
        [Required]
        public string? PatientName { get; set; }
		// Edad de paciente
        [StringLength(4)] // Formato AS de DICOM para la edad, ejemplo "034Y"
        public string? PatientAge { get; set; }
        // Sexo de paciente
        [StringLength(1),RegularExpression(@"[MFUO]")] // Incluye 'U' para desconocido y 'O' para otro
        public string? PatientSex { get; set; }
        //Peso del paciente
        [RegularExpression(@"\d{1,3}(\.\d{1})?")] // Formato para peso en kg con un decimal
        public string? PatientWeight { get; set; }
        //Fecha de nacimiento
        public DateTime? PatientBirthDate { get; set; } // Hacer nullable para manejar casos sin esta información
        // Campos adicionales sugeridos por la estructura DICOM
        [StringLength(64)]
        public string? IssuerOfPatientID { get; set; } //el emisor del PatientID,
        // Coleccion de estudios
    }
}
