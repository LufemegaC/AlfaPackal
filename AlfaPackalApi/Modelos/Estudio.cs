using static AlfaPackalApi.Modelos.ListaDeTrabajo;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using static Utileria.Listados;

namespace AlfaPackalApi.Modelos
{
    public class Estudio : IAuditable
    {
        //Llave primaria
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EstudioID { get; set; }
        //Paciente
        public int? PacienteID { get; set; }
        [ForeignKey("PacienteID")]
        public virtual Paciente Paciente { get; set; }
        //Doctor
        public int? DoctorID { get; set; }
        [ForeignKey("DoctorID")]
        public virtual Doctor Doctor { get; set; }
        //Lista de trabajo
        public int? ListaID { get; set; }
        [ForeignKey("ListaID")]
        public virtual ListaDeTrabajo ListaDeTrabajo { get; set; }
        //Modalidad
        [Required, MaxLength(2)]
        public Modalidad Modality { get; set; }
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
        //public virtual ICollection<ListaDeTrabajo> ListaDeTrabajos { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
