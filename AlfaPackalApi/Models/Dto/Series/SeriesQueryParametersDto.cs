namespace Api_PACsServer.Models.Dto.Series
{
    /// <summary>
    /// DTO for series-level query parameters.
    /// Inherits common parameters from BaseQueryParametersDto.
    /// </summary>
    public class SeriesQueryParametersDto : BaseQueryParametersDto
    {
        public string? SeriesInstanceUID { get; set; }
        public string? SeriesNumber { get; set; }
        public string? Modality { get; set; }
        public string? SeriesDateTime { get; set; }
        public string? PatientPosition { get; set; }
    }
}
