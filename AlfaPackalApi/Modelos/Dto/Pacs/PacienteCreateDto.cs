using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Utileria.Listados;

namespace AlfaPackalApi.Modelos.Dto.Pacs
{
    public class PacienteCreateDto
    {
        // ID del paciente DICOM/Metadato
        [Required]
        public int PatientID { get; set; }
        // Nombre del paciente/Metadato
        // Composicion FirstName+MiddleName+LastName+Prefix+Suffix
        [Required, MaxLength(100)]
        public string? PatientName { get; set; }
        // Edad del paciente DICOM/Metadato
        public string? PatientAge { get; set; }
        // Sexo del paciente DICOM/Metadato
        public string? PatientSex { get; set; }
        // Peso del paciente DICOM/Metadato
        public string? PatientWeight { get; set; }
        // Fecha de nacimiento
        public DateTime PatientBirthDate { get; set; }
    }
}
