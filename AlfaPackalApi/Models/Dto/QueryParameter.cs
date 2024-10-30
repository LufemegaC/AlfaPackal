using Api_PACsServer.Utilities;

namespace Api_PACsServer.Models.Dto
{
    public class QueryParameter
    {
        public string? Value { get; set; }
        public QueryOperators.SqlOperator Operator { get; set; }
    }
}
