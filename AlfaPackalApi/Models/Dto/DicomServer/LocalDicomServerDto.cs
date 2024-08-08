namespace Api_PACsServer.Models.Dto.DicomServer
{
    public class LocalDicomServerDto
    {
        public string IP { get; set; }
        public string AETitle { get; set; }
        public int Port { get; set; }
        public string Description { get; set; }
    }
}
