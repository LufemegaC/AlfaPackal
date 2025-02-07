using Api_PACsServer.Models.Dto.Base;
using Api_PACsServer.Models.Dto.DicomWeb.Qido;
using FellowOakDicom.Network;
using System.Text.RegularExpressions;

namespace Api_PACsServer.Models.Dto.Series
{
    public class SerieQueryParametersDto : BaseDicomQueryParametersDto
    {
        public QueryParameter? SeriesNumber { get; set; }
        public QueryParameter? SeriesDescription { get; set; }
        public QueryParameter? BodyPartExamined { get; set; }
        public QueryParameter? PatientPosition { get; set; }
        public QueryParameter? ProtocolName { get; set; }
        protected override Type EntityType => typeof(Serie); // Ajusta al nombre de tu clase de entidad en la BD


        public SerieQueryParametersDto()
        {
            DicomQueryLevel = DicomQueryRetrieveLevel.Series;
        }

        /// <summary>
        /// Validates the derived parameters specific to StudyQueryParametersDto.
        /// </summary>
        protected override void ValidateDerivedParameters()
        {
            // Validate Modality 
            if (Modality != null && !Regex.IsMatch(Modality.Value, "^[A-Z0-9]+$"))
            {
                throw new ArgumentException("Invalid Modality value. Expected alphanumeric uppercase characters.");
            }

            // Validate SeriesNumber 
            if (SeriesNumber != null && (!int.TryParse(SeriesNumber.Value, out _) || int.Parse(SeriesNumber.Value) <= 0))
            {
                throw new ArgumentException("Invalid SeriesNumber. Expected a positive integer.");
            }

            // Validate BodyPartExamined 
            if (BodyPartExamined != null && !Regex.IsMatch(BodyPartExamined.Value, "^[A-Z0-9_]+$"))
            {
                throw new ArgumentException("Invalid BodyPartExamined value. Expected alphanumeric or underscore characters.");
            }

            // Validate Laterality (if provided)
            //if (Laterality != null && !new[] { "R", "L", "U", "B" }.Contains(Laterality.Value.ToUpper()))
            //{
            //    throw new ArgumentException("Invalid Laterality value. Allowed values are: R (Right), L (Left), U (Unknown), B (Both).");
            //}
        }
    }
}
