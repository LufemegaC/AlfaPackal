using Api_PACsServer.Models.Dto.Base;

namespace Api_PACsServer.Models.Dto.DicomWeb.Qido
{
    public class QueryRequestParameters<T> where T : BaseDicomQueryParametersDto
    {
        public AdditionalParameters AdditionalParameters { get; set; }
        public T DicomParameters { get; set; }


        public QueryRequestParameters(AdditionalParameters additionalParameters, T dicomParameters)
        {
            AdditionalParameters = additionalParameters;
            DicomParameters = dicomParameters;
        }
    }
}
