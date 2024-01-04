using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Utileria.Listados;

namespace InterfazBasica.Models.Pacs
{
    public class PacienteDto
    {
        // ID del paciente
        [Key]
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
        public Genero Genero { get; set; }
        //public virtual ICollection<Estudio> Estudios { get; set; }
    }
}
