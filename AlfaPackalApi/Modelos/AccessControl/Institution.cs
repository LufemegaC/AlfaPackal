using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Api_PACsServer.Modelos.Geografia;

namespace Api_PACsServer.Modelos.AccessControl
{
    public class Institution
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string AssignedInstitutionName { get; set; }
        public string IdCiudad { get; set; }

        [ForeignKey("IdCiudad")]
        public virtual Ciudad Ciudad { get; set; }


    }
}
