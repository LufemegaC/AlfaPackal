namespace Api_PACsServer.Models.Dto
{
    /// <summary>
    /// Base DTO for common DICOM query parameters.
    /// </summary>
    public class BaseQueryParametersDto
    {
        public string? PatientID { get; set; }
        public string? StudyInstanceUID { get; set; }
    }
}
