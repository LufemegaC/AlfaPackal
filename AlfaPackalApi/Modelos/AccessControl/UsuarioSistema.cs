using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Modelos.AccessControl
{
    public class UsuarioSistema : IdentityUser
    {
        public string FullName { get; set; }
        public string Rol { get; set; }
        [Required]
        public int InstitutionId { get; set; }
        [ForeignKey("InstitutionId")]
        public virtual Institution Institution { get; set; }   
    }
}
