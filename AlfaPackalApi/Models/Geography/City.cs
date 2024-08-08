using System.ComponentModel.DataAnnotations.Schema;

namespace Api_PACsServer.Modelos.Geography
{
    /// <remarks>
    /// This class is missing its CRUD (Create, Read, Update, Delete) operations implementation.
    /// </remarks>
    public class City
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string StateID { get; set; }
        [ForeignKey("StateID ")]
        public virtual State State { get; set; }
    }
}
