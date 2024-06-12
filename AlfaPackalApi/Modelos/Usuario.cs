using Api_PACsServer.Modelos.AccessControl;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_PACsServer.Modelos
{
    public class Usuario
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Nombres { get; set; }

        public string Password { get; set; }

        public string Rol { get; set; }

        [Required]
        public int InstitutionId { get; set; }
        [ForeignKey("InstitutionId")]
        public virtual Institution Institution { get; set; }

    }
}
