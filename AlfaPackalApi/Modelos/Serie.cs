using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlfaPackalApi.Modelos
{
    public class Serie : IAuditable
    {
        // ID interna autoicrementable generada automaticamente 
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PACS_SerieID { get; set; } //ID interno
        // ID interno de Estudio padre, union con Estudio
        public int? PACS_EstudioID { get; set; }
        [ForeignKey("PACS_EstudioID")]
        public virtual Estudio Estudio { get; set; }  
        // UID metadato de estudio padre, union con Estudio
        [Required]
        public string StudyInstanceUID { get; set; }
        // Descripcion
        public string SeriesDescription { get; set; }
        // UID de instancia   
        [Required, MaxLength(64)]
        public string SeriesInstanceUID { get; set; } 
        //Numero de serie
        public int? SeriesNumber { get; set; }
        // Fecha y hora de inicio de la serie.
        public DateTime? SeriesDateTime { get; set; } 
        // Modalidad de la serie.
        [Required, MaxLength(16)]
        public string Modality { get; set; } 
        // Body Part Examined - parte del cuerpo examinada.
        [MaxLength(16)]
        public string BodyPartExamined { get; set; } 
        // Patient Position - posición del paciente.
        [MaxLength(16)]
        public string PatientPosition { get; set; }     
        // Imagenes de la serie
        public virtual ICollection<Imagen> Imagenes { get; set; }
        // Campos de control
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
