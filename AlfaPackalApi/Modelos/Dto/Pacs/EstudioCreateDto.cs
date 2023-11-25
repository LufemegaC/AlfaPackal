using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlfaPackalApi.Modelos.Dto.Pacs
{
    public class EstudioCreateDto
    {
        //Paciente
        [Required, ForeignKey("Paciente")]
        public int? PacienteID { get; set; }
        public virtual Paciente Paciente { get; set; }

        //Doctor
        [Required, ForeignKey("Doctor")]
        public int? DoctorID { get; set; }
        public virtual Doctor Doctor { get; set; }
        // Lista de Trabajo
        [Required, ForeignKey("ListaDeTrabajo")]
        public int ListaID { get; set; }
        public virtual ListaDeTrabajo ListaDeTrabajo { get; set; }
        //Modalidad
        [Required, MaxLength(50)]
        public string Modality { get; set; }
        // Descripcion
        [Required]
        public string DescripcionEstudio { get; set; }
        [Required]
        public DateTime StudyDate { get; set; }
        [Required]
        public int TiempoEstudio { get; set; }

        [Required, MaxLength(100)]
        public string BodyPartExamined { get; set; }
        [Required, MaxLength(64)]
        public string StudyInstanceUID { get; set; }
        [Required, MaxLength(16)]
        public string AccessionNumber { get; set; }
        public virtual ICollection<Serie> Series { get; set; }

    }
}
