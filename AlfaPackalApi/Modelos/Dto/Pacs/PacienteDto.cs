using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Utileria.DicomValues;

namespace AlfaPackalApi.Modelos.Dto.Pacs
{
    public class PacienteDto
    {
        // ID del paciente
        [Required,Display(Name = "Id del Paciente")]
        public string PatientID { get; set; } // ID del paciente DICOM/Metadato
        // Nombre del paciente/Metadato
        [Display(Name = "Nombre del Paciente"),Required]
        public string? PatientName { get; set; }
        [Display(Name = "Edad"),StringLength(4)]
        // Edad del paciente DICOM/Metadato
        public string? PatientAge { get; set; }
        // Sexo del paciente DICOM/Metadato
        [Display(Name = "Sexo"),StringLength(1)]
        public PatientSex PatientSex { get; set; }
        // Peso del paciente DICOM/Metadato
        [Display(Name = "Peso"),RegularExpression(@"\d{1,3}(\.\d{1})?")]
        public string? PatientWeight { get; set; }
        [Display(Name = "Fecha de Nacimiento")]
        // Fecha de nacimiento
        public DateTime? PatientBirthDate { get; set; } // Hacer nullable para manejar casos sin esta información
        [Display(Name = "Emisor del paciente"),StringLength(64)]
        public string IssuerOfPatientID { get; set; } //el emisor del PatientID,
    }
}
