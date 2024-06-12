using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace InterfazDeUsuario.Models.Pacs
{
    public class SerieDto
    {
        // Descripcion
        public string SeriesDescription { get; set; }// Descripcion
        [Required, MaxLength(64), Display(Name = "Id de Instancia")]
        public string SeriesInstanceUID { get; set; }
        // Numero de serie en el estudio
        [Display(Name = "Numero de Serie")]
        // Identificador de serie
        public int? SeriesNumber { get; set; }
        [Display(Name = "Fecha")]
        public DateTime? SeriesDateTime { get; set; } // Fecha y hora de inicio de la serie.
        [Required, MaxLength(16), Display(Name = "Modalidad")]
        public string Modality { get; set; } // Modalidad de la serie.
        [MaxLength(16), Display(Name = "Parte del cuerpo")]
        public string BodyPartExamined { get; set; } // Body Part Examined - parte del cuerpo examinada.
        [MaxLength(16), Display(Name = "Posicion del paciente")]
        public string PatientPosition { get; set; }// Patient Position - posición del paciente.
    }
}
