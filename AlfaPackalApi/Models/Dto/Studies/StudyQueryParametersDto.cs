namespace Api_PACsServer.Models.Dto.Studies
{
    /// <summary>
    /// DTO for study-level query parameters.
    /// Inherits common parameters from BaseQueryParametersDto.
    /// </summary>
    public class StudyQueryParametersDto : BaseQueryParametersDto
    {
        public string? StudyDate { get; set; }
        public string? AccessionNumber { get; set; }
        public string? PatientName { get; set; }
        public string? PatientAge { get; set; }
        public string? PatientSex { get; set; }
        public string? InstitutionName { get; set; }
        public string? BodyPartExamined { get; set; }
    }
}
