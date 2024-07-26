using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Utileria.DicomValues;

namespace AlfaPackalApi.Modelos.Dto.Pacs
{
    public class EstudioCreateDto
    {
        // ID de estudio Dicom/Metadato
        [Required]
        public string StudyInstanceUID { get; set; }
        // ID interno de paciente 
        [Required]
        public int? PACS_PatientID { get; set; }
        [StringLength(64)]
        // ID patient de maetadatos
        public string GeneratedPatientID { get; set; } // ID del paciente DICOM/Metadato
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
        // Informacion de participantes en el estudio
        public string? PerformingPhysicianName { get; set; } //Medico
        public string? OperatorName { get; set; }//Operador
        //Parametros tecnicos
        public string? ExposureTime { get; set; } //Tiempo de exposicion 
        public string? KVP { get; set; }
        // 09/07/24 Luis Felipe MG: Agrego las propiedades 
        // del proceso de carga
        // numero de archios dcm relacionados al estudio
        public int? NumberOfFiles { get; set; }
        // Tamaño total del estudio en MB
        public decimal TotalFileSizeMB { get; set; }
        // Número de series
        public int? NumberOfSeries { get; set; }
    }
}
