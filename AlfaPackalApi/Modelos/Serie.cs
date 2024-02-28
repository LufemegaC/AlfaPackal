using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlfaPackalApi.Modelos
{
    public class Serie : IAuditable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PACS_SerieID { get; set; } //ID interno


        public int? PACS_EstudioID { get; set; }
        [ForeignKey("PACS_EstudioID")]
        public virtual Estudio Estudio { get; set; }  

        public string SeriesDescription { get; set; }// Descripcion

        [Required, MaxLength(64)]
        public string SeriesInstanceUID { get; set; } // Identificador  

        public int? SeriesNumber { get; set; }

        public DateTime? SeriesDateTime { get; set; } // Fecha y hora de inicio de la serie.

        [Required, MaxLength(16)]
        public string Modality { get; set; } // Modalidad de la serie.

        [MaxLength(16)]
        public string BodyPartExamined { get; set; } // Body Part Examined - parte del cuerpo examinada.

        [MaxLength(16)]
        public string PatientPosition { get; set; }// Patient Position - posición del paciente.

        // Imagenes de la serie
        public virtual ICollection<Imagen> Imagenes { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
