using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;
using static Utileria.DicomValues;

namespace AlfaPackalApi.Modelos
{
    public class Paciente : IAuditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PACS_PatientID { get; set; } // ID interno/sistema

        [Required]
        [StringLength(64)] // Ajuste para cumplir con la longitud máxima común en sistemas DICOM
        public string PatientID { get; set; } // ID del paciente DICOM/Metadato

        // Se descompone PatientName en componentes según las recomendaciones DICOM
        [Required]
        public string? PatientName { get; set; }
		
		[Required, MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string MiddleName { get; set; }
        [Required, MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(20)]
        public string Prefix { get; set; }
        [MaxLength(20)]
        public string Suffix { get; set; }

        [StringLength(4)] // Formato AS de DICOM para la edad, ejemplo "034Y"
        public string? PatientAge { get; set; }

        [StringLength(1)]
        [RegularExpression(@"[MFUO]")] // Incluye 'U' para desconocido y 'O' para otro

        public PatientSex PatientSex { get; set; }
        [RegularExpression(@"\d{1,3}(\.\d{1})?")] // Formato para peso en kg con un decimal

        public string? PatientWeight { get; set; }

        public DateTime? PatientBirthDate { get; set; } // Hacer nullable para manejar casos sin esta información
        // Campos adicionales sugeridos por la estructura DICOM

        [StringLength(64)]
        public string IssuerOfPatientID { get; set; } //el emisor del PatientID,

        public virtual ICollection<Estudio> Estudios { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UpdateDate  { get; set; }
    }
}
