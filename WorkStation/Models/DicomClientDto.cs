namespace WorkStation.Models
{
    public class DicomClientDto
    {
        // IP Address
        public string IP { get; set; }
        // SCU AETitle
        public string AETitle { get; set; }
        // SCP AETtitle
        public string CalledAE { get; set; }
        // local port
        public int Port { get; set; }
        
    }
}
