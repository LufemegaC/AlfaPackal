using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlfaPackalApi.Modelos;

namespace Api_PACsServer.Modelos.Cargas
{
    public class ImagenCarga
    // 08/07/24.-LuisFelipe: Clase para almacenar informacion de carga de la entidad.
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PACS_ImagenCargaID { get; set; }
        // Union con la entidad principal
        public int PACS_ImagenID { get; set; }
        [Required, ForeignKey("PACS_ImagenID")]
        public virtual Imagen Imagen { get; set; }
        // Tamaño total del estudio en MB
        public decimal TotalFileSizeMB { get; set; }
        // Numero de Frames, Metadatos
        public int? NumberOfFrames { get; set; }
        // Datos de bitacora
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
