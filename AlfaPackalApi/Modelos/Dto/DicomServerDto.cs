namespace Api_PACsServer.Modelos.Dto
{
    public class DicomServerDto
    {
        public string IP { get; set; }
        public string AETitle { get; set; }
        public int PuertoRedLocal { get; set; }
        public string Description { get; set; }
        public int InstitutionId { get; set; }
    }
}
