using System.ComponentModel.DataAnnotations.Schema;

namespace Api_PACsServer.Modelos.Geografia
{
    public class Ciudad
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string IdEstado { get; set; }
        [ForeignKey("IdEstado")]
        public virtual Estado Estado { get; set; }
    }
}
