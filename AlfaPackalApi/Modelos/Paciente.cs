using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlfaPackalApi.Modelos
{
    public class Paciente : IAuditable
    {
        // ID del paciente
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PacienteID { get; set; }
        // Nombre del paciente
        [Required, MaxLength(100)]
        public string Nombre { get; set; }
        // Apellido del paciente
        [Required, MaxLength(100)]
        public string Apellido { get; set; }
        // Fecha de Nacimiento
        [Required]
        public DateTime FechaNacimiento { get; set; }
        // Genero/Sexo
        [Required, MaxLength(1)]
        public string Genero { get; set; }
        public virtual ICollection<Estudio> Estudios { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
