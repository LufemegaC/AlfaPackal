using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Models.AccessControl
{
    /// <remarks>
    /// This class is missing its CRUD (Create, Read, Update, Delete) operations implementation.
    /// </remarks>
    public class LocalDicomServer
    {
        // Represents the unique identifier for the entity.
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // IP address, required and limited to 39 characters.
        [Required, StringLength(39)]
        public string IP { get; set; }

        // AE Title, required and limited to 16 characters.
        [Required, StringLength(16)]
        public string AETitle { get; set; }

        // Represents the local port number.
        public int Port { get; set; }

        // description
        [Required, StringLength(100)]
        public string Description { get; set; }


        [Required]
        public bool IsActive { get; set; }

        //// foreign key to the institution.
        //public int InstitutionId { get; set; }
        //[ForeignKey("InstitutionId")]
        //public virtual Institution Institution { get; set; }


    }
}
