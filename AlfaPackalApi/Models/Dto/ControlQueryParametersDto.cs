namespace Api_PACsServer.Models.Dto
{
    public class ControlQueryParametersDto
    {
        public string? Limit { get; set; }
        public string? Order { get; set; }
        public string? Offset { get; set; }
        public List<string?> IncludeFields { get; set; }
        public string? Format { get; set; }
        public string? Page { get; set; }
        public string? PageSize { get; set; }
    }
}
