using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Utileria.Listados;

namespace InterfazBasica.Models.Pacs
{
    public class EstudioDto
    {   // 17/01/24 Luis Felipe MG .- Agrego los DisplayName
        // ID de estudio Dicom/Metadato
        [Required, Display(Name = "ID")]
        public string StudyInstanceUID { get; set; }
        [Display(Name = "Cometarios")]
        // Cometarios del Estudio
        public string? StudyComments { get; set; }
        [Display(Name = "Modalidad")]
        // Modalidad de estudio Dicom/Metadato
        [Required]
        public string? Modality { get; set; }
        [Display(Name = "Descripción")]
        // Descripcion del estudio DICOM/Metadatos
        public string? StudyDescription { get; set; }
        // Fecha en que realizó el estudio DICOM/Metadatos
        [Required,Display(Name = "Fecha de estudio")]
        public DateTime StudyDate { get; set; }
        // Parte del cuerpo examinada DICOM/Metadatos
        [Display(Name = "Parte del cuerpo examinada")]
        public string? BodyPartExamined { get; set; }
        // Numero de accesso DICOM/Metadato
        [Required, MaxLength(64), Display(Name = "Numero de accesso")]
        public string AccessionNumber { get; set; }
        [Display(Name = "Personal medico")]
        // Informacion de participantes en el estudio
        public string? PerformingPhysicianName { get; set; } //Medico
        [Display(Name = "Tecnico operador")]
        public string? OperatorName { get; set; }//Operador
        [Display(Name = "Tiempo de exposicion")]
        //Parametros tecnicos
        public string? ExposureTime { get; set; } //Tiempo de exposicion 
        [Display(Name = "KVP")]
        public string? KVP { get; set; }
        [Display(Name = "Numero de imagenes")]
        // Numero de Frames/Imagenes
        public int? NumberOfFrames { get; set; }
    }
}
