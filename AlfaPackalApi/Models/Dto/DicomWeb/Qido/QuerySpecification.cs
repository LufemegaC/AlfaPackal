using Microsoft.Data.SqlClient;

namespace Api_PACsServer.Models.Dto.DicomWeb.Qido
{
    public class QuerySpecification
    {
        public QuerySpecification(string selectClause,string fromClause, string whereClause,
                                  string additionalClause = "", List<SqlParameter> parameters = null
                                  ,bool includeFieldsSpecified = false)
        {
            SelectClause = selectClause ?? throw new ArgumentNullException(nameof(selectClause));
            FromClause = fromClause ?? throw new ArgumentNullException(nameof(fromClause));
            WhereClause = whereClause ?? throw new ArgumentNullException(nameof(whereClause));
            AdditionalClause = additionalClause;
            Parameters = parameters ?? new List<SqlParameter>();
            IncludeFieldsSpecified = includeFieldsSpecified;
        }
        public bool IncludeFieldsSpecified { get; set; }
        public string SelectClause { get; set; }
        public string FromClause { get; set; }
        public string WhereClause { get; set; }
        public string AdditionalClause { get; set; }
        public List<SqlParameter> Parameters { get; set; }
    }
}
