namespace WorkStation.Models
{
    public class DicomServerDto
    {
        public string IP { get; set; }
        public string AETitle { get; set; }
        public int PuertoRedLocal { get; set; }
        public string Description { get; set; }
        //Union con institution
    }
}
