
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using static Utileria.DicomValues;
using System.Diagnostics;
using Newtonsoft.Json;
using Api_PACsServer.Modelos.AccessControl;

namespace AlfaPackalApi.Modelos
{
    public class Estudio : IAuditable
    {
        //Llave primaria
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // ID Interno de estudio
        public int PACS_EstudioID { get; set; } 
        // ID de estudio Dicom/Metadato
        [Required]
        public string StudyInstanceUID { get; set; }
        // ID interno de paciente 
        [Required]
        public int PACS_PatientID { get; set; }
        [ForeignKey("PACS_PatientID")]
        public virtual Paciente Paciente { get; set; }
        // ID (Metadato) de paciente 
        [Required,StringLength(64)]
        // ID patient de maetadatos
        public string GeneratedPatientID { get; set; }
        // Patiente ID generado por Servidor DICOM
        // Comentarios de estudios
        public string? StudyComments { get; set; }
        // Modalidad de estudio Dicom/Metadato
        [Required]
        public string? Modality { get; set; }
        // Descripcion del estudio DICOM/Metadatos
        public string? StudyDescription { get; set; }
        // Fecha en que realizó el estudio DICOM/Metadatos
        [Required]
        public DateTime StudyDate { get; set; }
        // Parte del cuerpo examinada DICOM/Metadatos
        public string? BodyPartExamined { get; set; }
        // Numero de accesso DICOM/Metadato
        [Required, MaxLength(64)]  
        public string AccessionNumber { get; set; }
        // dirección del archivo DICOM
        public string? DicomFileLocation { get; set; }
        // Nombre de institucion que realiza el estudio DICOM/Metadatos
        public string? InstitutionName { get; set; }
        // ** Institucion emisora
        [Required]
        public int InstitutionId { get; set; }
        [ForeignKey("InstitutionId")]
        public virtual Institution Institution { get; set; }
        // Informacion de participantes en el estudio
        public string? PerformingPhysicianName { get; set; } //Medico
        public string? OperatorName { get; set; }//Operador
        //Parametros tecnicos
        public string? ExposureTime { get; set; } //Tiempo de exposicion 
        public string? KVP { get; set; }
        // Numero de Frames/Imagenes
        public int? NumberOfFrames { get; set; }
        // Comentarios de imagen
        public virtual ICollection<Serie> Series { get; set; }
        // Datos de bitacora
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
