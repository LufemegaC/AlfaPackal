using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlfaPackalApi.Modelos.Dto.Roles
{
    public class DoctorCreateDto
    {
        public string UserID { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; }

        [Required, MaxLength(100)]
        public string Apellido { get; set; }

        [Required, MaxLength(100)]
        public string Especialidad { get; set; }

        public virtual ICollection<Estudio> Estudios { get; set; }
    }
}
