namespace Api_PACsServer.Models.OHIFVisor
{
    public class OHIFSerie
    {
        public string SeriesInstanceUID { get; set; }
        public string Modality { get; set; }
        public string SeriesNumber { get; set; }
        public string SeriesDescription { get; set; }
        public List<OHIFInstance> Instances { get; set; }  // Lista de instancias (imágenes) asociadas a la serie
    }
}
