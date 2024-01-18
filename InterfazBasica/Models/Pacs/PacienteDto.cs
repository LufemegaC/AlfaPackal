using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Utileria.Listados;

namespace InterfazBasica.Models.Pacs
{
    public class PacienteDto
    {
        // ID del paciente
        [Required,Display(Name = "Id del Paciente")]
        public int PatientID { get; set; }
        // Nombre del paciente/Metadato
        [Display(Name = "Nombre del Paciente")]
        [Required, MaxLength(100)]
        public string? PatientName { get; set; }
        [Display(Name = "Edad")]
        // Edad del paciente DICOM/Metadato
        public string? PatientAge { get; set; }
        // Sexo del paciente DICOM/Metadato
        [Display(Name = "Sexo")]
        public string? PatientSex { get; set; }
        // Peso del paciente DICOM/Metadato
        [Display(Name = "Peso")]
        public string? PatientWeight { get; set; }
        [Display(Name = "Fecha de Nacimiento")]
        // Fecha de nacimiento
        public DateTime PatientBirthDate { get; set; }
    }
}
