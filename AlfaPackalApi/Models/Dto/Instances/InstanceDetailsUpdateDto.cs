namespace Api_PACsServer.Models.Dto.Instances
{
    public class InstanceDetailsUpdateDto
    {
        public InstanceDetailsUpdateDto(string instanceUID, string fileLocation)
        {
            SOPInstanceUID = instanceUID;
            FileLocation = fileLocation;
            UpdateDate = DateTime.UtcNow;
        }
        //pk
        public string SOPInstanceUID { get; set; }
        // URL of the Instance location
        public string? FileLocation { get; set; } // e.g., the URL of the image in Azure
        public DateTime UpdateDate { get; set; }
    }
}
