using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlfaPackalApi.Modelos.Dto.Pacs
{
    public class SerieCreateDto
    {
        

        //Estudio
        public int? PACS_EstudioID { get; set; }
        [Required]
        public string StudyInstanceUID { get; set; }
        // Descripcion
        public string SeriesDescription { get; set; }
        // Identificador
        // Numero de serie en el estudio
		[Required, MaxLength(64)]
        public string SeriesInstanceUID { get; set; }
        public int? SeriesNumber { get; set; }
        public DateTime? SeriesDateTime { get; set; } // Fecha y hora de inicio de la serie.
        [Required, MaxLength(16)]
        public string Modality { get; set; } // Modalidad de la serie.

        [MaxLength(16)]
        public string BodyPartExamined { get; set; } // Body Part Examined - parte del cuerpo examinada.

        [MaxLength(16)]
        public string PatientPosition { get; set; }// Patient Position - posición del paciente.
    }
}
