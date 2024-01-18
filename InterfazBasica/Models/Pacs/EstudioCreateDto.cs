using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Utileria.Listados;

namespace InterfazBasica.Models.Pacs
{
    public class EstudioCreateDto
    {
        // ID de estudio Dicom/Metadato
        [Required]
        public string StudyInstanceUID { get; set; }
        // ID interno de paciente 
        public int? PACS_PatientID { get; set; }
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
        // Nombre de institucion que realiza el estudio DICOM/Metadatos
        public string? InstitutionName { get; set; }
        // Informacion de participantes en el estudio
        public string? PerformingPhysicianName { get; set; } //Medico
        public string? OperatorName { get; set; }//Operador
        //Parametros tecnicos
        public string? ExposureTime { get; set; } //Tiempo de exposicion 
        public string? KVP { get; set; }
        // Numero de Frames/Imagenes
        public int? NumberOfFrames { get; set; }
    }
}
