using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;
using static Utileria.DicomValues;

namespace AlfaPackalApi.Modelos
{
    public class Paciente : IAuditable
    {
        // ID interno autoincrementable de Paciente
        [JsonProperty("PACS_PatientID"),Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PACS_PatientID { get; set; } // ID interno/sistema
        // ID Paciente (Metadato) 
        [StringLength(64)]        
        public string? PatientID { get; set; } // ID del paciente DICOM/Metadato
        // ID de paciente generado por servidor DICOM
        [Required, StringLength(64)]
        public string GeneratedPatientID { get; set; }
        // Nombre completo de paciente 
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
        public virtual ICollection<Estudio> Estudios { get; set; }
        // Campos de control
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate  { get; set; }
    }
}
