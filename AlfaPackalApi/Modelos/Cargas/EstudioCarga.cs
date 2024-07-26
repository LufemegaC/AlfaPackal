using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlfaPackalApi.Modelos;

namespace Api_PACsServer.Modelos.Cargas
{
    public class EstudioCarga
    // 08/07/24.-LuisFelipe: Clase para almacenar informacion de carga de la entidad.
    // Se agregará a EntityFM, 

    {
        // Llave primaria autogenerada
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PACS_EstudioCargaID { get; set; }
        // Union con entidad princiapl
        public int PACS_EstudioID { get; set; }
        [Required, ForeignKey("PACS_EstudioID")]
        public virtual Estudio Estudio { get; set; }
        // numero de archios dcm relacionados al estudio
        public int? NumberOfFiles { get; set; }
        // Tamaño total del estudio en MB
        public decimal TotalFileSizeMB { get; set; }
        // Número de series
        public int? NumberOfSeries { get; set; }
        // Datos de bitacora
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
