using static AlfaPackalApi.Modelos.ListaDeTrabajo;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace AlfaPackalApi.Modelos
{
    public enum Modalidad
    {
        CT, // Tomografía Computarizada
        MR, // Resonancia Magnética
        US, // Ultrasonido
        XR, // Radiografía
        NM, // Medicina Nuclear
        DX, // Radiografía Digital
        MG, // Mamografía
        IO, // Imagen Intraoral
        OT  // Otro
    }
    public class Estudio : IAuditable
    {
        //Llave primaria
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EstudioID { get; set; }
        //Paciente
        [Required, ForeignKey("Paciente")]
        public int? PacienteID { get; set; }
        public virtual Paciente Paciente { get; set; }

        //Doctor
        [Required, ForeignKey("Doctor")]
        public int? DoctorID { get; set; }
        public virtual Doctor Doctor { get; set; }
        //Lista de trabajo
        [Required, ForeignKey("ListaDeTrabajo")]
        public int ListaID { get; set; }
        public virtual ListaDeTrabajo ListaDeTrabajo { get; set; }

        //Modalidad
        [Required]
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
