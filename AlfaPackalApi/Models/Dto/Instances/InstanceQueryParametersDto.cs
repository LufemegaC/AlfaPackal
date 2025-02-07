using Api_PACsServer.Models.Dto.Base;
using Api_PACsServer.Models.Dto.DicomWeb.Qido;
using FellowOakDicom.Network;
using System.Text.RegularExpressions;

namespace Api_PACsServer.Models.Dto.Instances
{
    /// <summary>
    /// DTO for instance-level query parameters.
    /// Inherits common parameters from BaseQueryParametersDto.
    /// </summary>
    public class InstanceQueryParametersDto : BaseDicomQueryParametersDto
    {
        public QueryParameter? SOPClassUID { get; set; }
        public QueryParameter? TransferSyntaxUID { get; set; }
        public QueryParameter? InstanceNumber { get; set; }
        public QueryParameter? ImageComments { get; set; }
        protected override Type EntityType => typeof(Instance); // Ajusta al nombre de tu clase de entidad en la BD

        public InstanceQueryParametersDto()
        {
            DicomQueryLevel = DicomQueryRetrieveLevel.Image;
        }

        /// <summary>
        /// Validates the derived parameters specific to InstanceQueryParametersDto.
        /// </summary>
        protected override void ValidateDerivedParameters()
        {
            // Ensure InstanceNumber is a positive integer
            if (InstanceNumber != null && (!int.TryParse(InstanceNumber.Value, out _) || int.Parse(InstanceNumber.Value) <= 0))
            {
                throw new ArgumentException("Invalid InstanceNumber. Expected a positive integer.");
            }
        }
    }
}
