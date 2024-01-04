using System.ComponentModel.DataAnnotations;

namespace InterfazBasica.Models.Roles
{
    public class DoctorDto
    {
        [Key]
        public int DoctorID { get; set; }
        public string UserID { get; set; }
        [Required, MaxLength(100)]
        public string Nombre { get; set; }
        [Required, MaxLength(100)]
        public string Apellido { get; set; }
        [Required, MaxLength(100)]
        public string Especialidad { get; set; }
       // public virtual ICollection<Estudio> Estudios { get; set; }
    }
}
