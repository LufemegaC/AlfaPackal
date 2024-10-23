namespace Api_PACsServer.Models.Dto.Instances
{
    /// <summary>
    /// DTO for instance-level query parameters.
    /// Inherits common parameters from BaseQueryParametersDto.
    /// </summary>
    public class InstanceQueryParametersDto : BaseQueryParametersDto
    {
        public string? SOPInstanceUID { get; set; }
        public string? SOPClassUID { get; set; }
        public string? TransferSyntaxUID { get; set; }
        public string? InstanceNumber { get; set; }
        public string? ImageComments { get; set; }
        public string? PhotometricInterpretation { get; set; }
        public string? PixelSpacing { get; set; }
        public string? NumberOfFrames { get; set; }
        public string? ImagePositionPatient { get; set; }
        public string? ImageOrientationPatient { get; set; }
    }
}
