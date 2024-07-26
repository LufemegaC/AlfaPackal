using AlfaPackalApi.Modelos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Modelos.Cargas
{
    public class SerieCarga
    // 08/07/24.-LuisFelipe: Clase para almacenar informacion de carga de la entidad.
    {
        // Llave princiapal autogenerada
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PACS_SerieCargaID { get; set; }
        // Union con entidad princiapal
        public int PACS_SerieID { get; set; }
        [Required, ForeignKey("PACS_SerieID")]
        public virtual Serie Serie { get; set; }
        // Numero de imagenes relacionadas
        public int? NumberOfImages { get; set; }
        // Tamaño total del estudio en MB
        public decimal TotalFileSizeMB { get; set; }
        // Datos de bitacora
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
