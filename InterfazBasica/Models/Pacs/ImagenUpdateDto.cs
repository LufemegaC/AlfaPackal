using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InterfazBasica.Models.Pacs
{
    public class ImagenUpdateDto
    {
        // 16/01/2024 Luis Felipe MG.- Comento toda la clase por que el UPDATE
        // no estará presente en esta version

        //// PK de la imagen
        //[Key]
        //public int ImagenID { get; set; }
        //[Required]
        //public string ImageDescription { get; set; } //Descripción de la imagen
        //// Serie
        //public int SerieID { get; set; }
        //// UID
        //[Required, MaxLength(64)]
        //public string SOPInstanceUID { get; set; }
        //// Numero de la imagen dentro de la serie
        //public int ImageNumber { get; set; }
        //// Direccion URL de la imagen
        //[Required]
        //public string ImageLocation { get; set; }
    }
}
