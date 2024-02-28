using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Utileria.DicomValues;

namespace InterfazBasica.Models.Pacs
{
    public class PacienteCreateDto
    {
        // ID del paciente DICOM/Metadato
        [Required]
        [StringLength(64)] // Ajuste para cumplir con la longitud máxima común en sistemas DICOM
        public string PatientID { get; set; } // ID del paciente DICOM/Metadato
        // Nombre del paciente/Metadato
        // Composicion FirstName+MiddleName+LastName+Prefix+Suffix
        [Required]
        public string? PatientName { get; set; }
        // Edad del paciente DICOM/Metadato
        [StringLength(4)] // Formato AS de DICOM para la edad, ejemplo "034Y"
        public string? PatientAge { get; set; }
        // Sexo del paciente DICOM/Metadato
        [StringLength(1)]
        [RegularExpression(@"[MFUO]")] // Incluye 'U' para desconocido y 'O' para otro
        public PatientSex PatientSex { get; set; }
        [RegularExpression(@"\d{1,3}(\.\d{1})?")] // Formato para peso en kg con un decimal

        public string? PatientWeight { get; set; }
        // Fecha de nacimiento
        public DateTime? PatientBirthDate { get; set; } // Hacer nullable para manejar casos sin esta información
        [StringLength(64)]
        public string IssuerOfPatientID { get; set; } //el emisor del PatientID,
    }
}
