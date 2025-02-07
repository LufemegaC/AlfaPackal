
namespace Api_PACsServer.Models
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Names { get; set; }

        public string Password { get; set; }

        public string Rol { get; set; }

        //[Required]
        //public int InstitutionId { get; set; }
        //[ForeignKey("InstitutionId")]
        //public virtual Institution Institution { get; set; }

    }
}
