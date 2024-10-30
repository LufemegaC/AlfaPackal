namespace Api_PACsServer.Models.Dto.Studies
{
    /// <summary>
    /// DTO for study-level query parameters.
    /// Inherits common parameters from BaseQueryParametersDto.
    /// </summary>
    public class StudyQueryParametersDto : BaseQueryParametersDto
    {
        public QueryParameter? StudyDate { get; set; }
        public QueryParameter? AccessionNumber { get; set; }
        public QueryParameter? PatientName { get; set; }
        public QueryParameter? PatientAge { get; set; }
        public QueryParameter? PatientSex { get; set; }
        public QueryParameter? InstitutionName { get; set; }
        public QueryParameter? BodyPartExamined { get; set; }
        public QueryParameter? Modality { get; set; }
    }
}
