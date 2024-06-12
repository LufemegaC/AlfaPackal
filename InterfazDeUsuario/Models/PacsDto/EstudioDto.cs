using System.ComponentModel.DataAnnotations;

namespace InterfazDeUsuario.Models.Pacs
{
    public class EstudioDto
    {

        // ID de estudio Dicom/Metadato        
        public string StudyInstanceUID { get; set; }
        public int? PACS_PatientID { get; set; }
        [Required, StringLength(64)]
        public string PatientID { get; set; } // ID del paciente DICOM/Metadato
        // Cometarios del Estudio
        public string? StudyComments { get; set; }
        // Modalidad de estudio Dicom/Metadato        
        public string? Modality { get; set; }
        // Descripcion del estudio DICOM/Metadatos
        public string? StudyDescription { get; set; }
        // Fecha en que realizó el estudio DICOM/Metadatos        
        public DateTime StudyDate { get; set; }
        // Parte del cuerpo examinada DICOM/Metadatos
        public string? BodyPartExamined { get; set; }
        // Numero de accesso DICOM/Metadato
        public string AccessionNumber { get; set; }
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
