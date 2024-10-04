namespace Api_PACsServer.Models.OHIFVisor
{
    public class OHIFInstance
    {
        public string SOPInstanceUID { get; set; }
        public string InstanceNumber { get; set; }
        public string TransferSyntaxUID { get; set; }
        public string ImagePositionPatient { get; set; }  // Si es una imagen, información de su posición
        public string ImageOrientationPatient { get; set; }  // Orientación de la imagen
        public string PixelSpacing { get; set; }
    }
}
