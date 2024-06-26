﻿using System.ComponentModel.DataAnnotations;

namespace DicomProcessingService.Dtos
{
    public class ImagenCreateDto
    {
        // Union con entidad Serie
        public int PACS_SerieID { get; set; }
        // UID
        [Required, MaxLength(64)]
        public string SOPInstanceUID { get; set; }
        // Numero de la imagen dentro de la serie
        public string ImageComments { get; set; } //Descripción de la imagen

        [Required, MaxLength(64)]
        public int ImageNumber { get; set; }
        // Direccion URL de la imagen
        public string? ImageLocation { get; set; } //Ubicación de la imagen (por ejemplo, la URL de la imagen en Azure)
        // Transfer Syntax UID para especificar la codificación de la imagen
        [Required, MaxLength(64)]
        public string TransferSyntaxUID { get; set; }

        // Photometric Interpretation para la interpretación de color de la imagen
        [MaxLength(16)]
        public string PhotometricInterpretation { get; set; }

        // Dimensiones de la imagen
        public int Rows { get; set; }
        public int Columns { get; set; }

        // Pixel Spacing para el espaciamiento físico de los píxeles en la imagen
        public string PixelSpacing { get; set; }
    }
}
