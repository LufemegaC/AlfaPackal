﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlfaPackalApi.Modelos
{
    public class Imagen : IAuditable
    {
        // PK de la imagen
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImagenID { get; set; }

        [Required]
        public string ImageDescription { get; set; } //Descripción de la imagen
        // Serie
        public int SerieID { get; set; }
        [Required, ForeignKey("SerieID")]
        public virtual Serie Serie { get; set; }
        // UID
        [Required, MaxLength(64)]
        public string SOPInstanceUID { get; set; }
        // Numero de la imagen dentro de la serie
        public int ImageNumber { get; set; }
        // Direccion URL de la imagen
        [Required]
        public string ImageLocation { get; set; } //Ubicación de la imagen (por ejemplo, la URL de la imagen en Azure)
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }

    }
}
