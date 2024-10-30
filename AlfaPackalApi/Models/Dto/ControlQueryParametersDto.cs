namespace Api_PACsServer.Models.Dto
{
    public class ControlQueryParametersDto
    {
        public QueryParameter? Limit { get; set; }
        public QueryParameter? OrderBy { get; set; }
        public QueryParameter? OrderDirection { get; set; }
        public QueryParameter? Offset { get; set; }
        public List<string?> IncludeFields { get; set; } = new List<string?>();
        public QueryParameter? Format { get; set; }
        public QueryParameter? Page { get; set; }
        public QueryParameter? PageSize { get; set; }
    }

}
