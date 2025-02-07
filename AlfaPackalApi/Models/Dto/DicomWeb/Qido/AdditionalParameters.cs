namespace Api_PACsServer.Models.Dto.DicomWeb.Qido
{
    public class AdditionalParameters
    {
        public QueryParameter? Limit { get; set; }
        public QueryParameter? OrderBy { get; set; }
        public QueryParameter? OrderDirection { get; set; }
        public QueryParameter? Offset { get; set; }
        public QueryParameter? Format { get; set; }
        public QueryParameter? Page { get; set; }
        public QueryParameter? PageSize { get; set; }

        public void ValidateParameters()
        {

        }

    }

}
