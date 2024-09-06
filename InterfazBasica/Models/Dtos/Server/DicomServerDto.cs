namespace InterfazBasica_DCStore.Models.Dtos.Server
{
    public class DicomServerDto
    {
        public string IP { get; set; }
        public string AETitle { get; set; }
        public int Port { get; set; }
        public string Description { get; set; }
        // Interface props
        public bool IsRunning { get; set; }

    }
}
