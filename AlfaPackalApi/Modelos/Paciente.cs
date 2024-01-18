using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;
using static Utileria.Listados;

namespace AlfaPackalApi.Modelos
{
    public class Paciente : IAuditable
    {
        // ID interno/sistema
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PACS_PatientID { get; set; } 
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
        public virtual ICollection<Estudio> Estudios { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
