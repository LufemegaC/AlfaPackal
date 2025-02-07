using Api_PACsServer.Models.Dto.Base;
using Api_PACsServer.Models.Dto.DicomWeb.Qido;
using FellowOakDicom.Network;
using System.Text.RegularExpressions;

namespace Api_PACsServer.Models.Dto.Studies
{
    /// <summary>
    /// DTO for study-level query parameters.
    /// Inherits common parameters from BaseQueryParametersDto.
    /// </summary>
    public class StudyQueryParametersDto : BaseDicomQueryParametersDto
    {
        public QueryParameter? StudyDate { get; set; }
        public QueryParameter? StudyDescription { get; set; }
        public QueryParameter? PatientName { get; set; }
        public QueryParameter? PatientAge { get; set; }
        public QueryParameter? PatientSex { get; set; }
        public QueryParameter? InstitutionName { get; set; }
        public QueryParameter? ReferringPhysicianName { get; set; }
        protected override Type EntityType => typeof(Study); // Ajusta al nombre de tu clase de entidad en la BD

        public StudyQueryParametersDto()
        {
            DicomQueryLevel = DicomQueryRetrieveLevel.Study;
        }


        /// <summary>
        /// Validates the derived parameters specific to StudyQueryParametersDto.
        /// </summary>
        protected override void ValidateDerivedParameters()
        {
            // Validate StudyDate (if provided)
            if (StudyDate != null && !DateTime.TryParse(StudyDate.Value, out _))
            {
                throw new ArgumentException("Invalid StudyDate format. Expected a valid date.");
            }

            // Validate PatientSex (if provided)
            if (PatientSex != null && !new[] { "M", "F", "O", "U" }.Contains(PatientSex.Value.ToUpper()))
            {
                throw new ArgumentException("Invalid PatientSex value. Allowed values are: M, F, O, U.");
            }

            // Validate PatientAge (if provided)
            if (PatientAge != null && !Regex.IsMatch(PatientAge.Value, "^\\d{3}[DWMY]$"))
            {
                throw new ArgumentException("Invalid PatientAge format. Expected format: 001D, 034Y, etc.");
            }

        }
    }
}
