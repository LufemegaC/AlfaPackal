using Api_PACsServer.Models.Dto.DicomWeb.Qido;
using Newtonsoft.Json.Linq;
using static Api_PACsServer.Utilities.QueryOperators;

namespace Api_PACsServer.Factories
{
    public class QueryParameterFactory
    {
        public static QueryParameter CreateQueryParameter(string columnName, string decodedValue)
        {
            // (1) Validar si se de tipo control y de ser ejecutar el metodo adecuado
            // enviando el value y retornar la instancia
            if (IsControlParameter(columnName))
            {
                var controlParameter = TryCreateControlParameter(columnName, decodedValue);
                if (controlParameter != null)
                {
                    return controlParameter;
                }
                else
                    throw new InvalidOperationException($"Cannot create control param.");
            }
            else
            {
                var normalizedColumn = columnName.ToLower();
                // Check if the column is in the searchable string attributes list
                if (SearchableStringAttributes.Contains(normalizedColumn))
                {
                    return QueryParameter.CreateSearchableStringParameter(decodedValue);
                }

                // Check if the column is in the exact string attributes list
                if (ExactStringAttributes.Contains(normalizedColumn))
                {
                    return QueryParameter.CreateExactStringParameter(decodedValue);
                }
                else
                {
                    var dataType = MappingConfig.StudyColumnTypeMapping.TryGetValue(columnName.ToLower(), out var mappedDataType)
                                    ? mappedDataType
                                    : QueryParameterType.String;
                    // Get operator and value, use to determinate SQL Operator
                    SqlOperator sqlOperator = OperatorMappingToSqlOperator(decodedValue);
                    // create instance of parameter
                    return new QueryParameter(decodedValue, sqlOperator, dataType, QueryParameterCategory.Dicom);
                }
            }
        }

        // Método auxiliar para mapear el operador
        private static SqlOperator OperatorMappingToSqlOperator(string value)
        {
            if (value.Contains("-")) //isRange
            {
                return SqlOperator.Between;
            }
            else if (value.Contains("*"))
            {
                return SqlOperator.Like;
            }
            var operatorKey = value.Substring(0, 1);
            return operatorKey.ToLower() switch
            {
                "=" => SqlOperator.Equals,
                ">" => SqlOperator.GreaterThan,
                "<" => SqlOperator.LessThan,
                ">=" => SqlOperator.GreaterThanOrEqual,
                "<=" => SqlOperator.LessThanOrEqual,
                "!" => SqlOperator.NotEqual,
                "orderby" => SqlOperator.OrderBy,
                "desc" => SqlOperator.Descending,
                "asc" => SqlOperator.Ascending,
                _ => SqlOperator.Equals
            };
        }

        // List of known control parameters (not part of DICOM tags)
        public static readonly List<string> ControlParameters = new List<string>
        {
            "limit",
            "orderby",
            "orderdirection",
            "offset",
            "includefields",
            "format",
            "page",
            "pagesize"
        };

        // List of known control parameters (not part of DICOM tags)
        public static readonly List<string> SearchableStringAttributes = new List<string>
        {
            // Study
            "studydescription",
            "patientname",
            "institutionname",
            "referringphysicianname",
            // Serie
            "protocolname",
            "seriesdescription",
            // Instance
            "imagecomments"
        };

        // List of known control parameters (not part of DICOM tags)
        public static readonly List<string> ExactStringAttributes = new List<string>
        { 
            // Patient
            "patientsex",
            // Serie
            "bodypartexamined",
            "patientposition",
        };






        // Method to check if a column name is a control parameter
        public static bool IsControlParameter(string columnName)
        {
            return ControlParameters.Contains(columnName.ToLower());
        }

        // Dictionary that maps control parameter names to their creation functions
        public static readonly Dictionary<string, Func<string, QueryParameter>> ControlParameterFactory = new Dictionary<string, Func<string, QueryParameter>>()
        {
            { "limit", value => QueryParameter.CreateLimitParameter(value) },
            { "orderby", value => QueryParameter.CreateOrderByParameter(value) },
            { "orderdirection", value => QueryParameter.CreateOrderDirectionParameter(value) },
            { "offset", value => QueryParameter.CreateOffsetParameter(value) },
            { "format", value => QueryParameter.CreateFormatParameter(value) },
            { "page", value => QueryParameter.CreatePageParameter(value) },
            { "pagesize", value => QueryParameter.CreatePageSizeParameter(value) }
        };

        // Method to determine if a column name is a control parameter and return the created QueryParameter
        public static QueryParameter? TryCreateControlParameter(string columnName, string value)
        {
            if (ControlParameterFactory.TryGetValue(columnName.ToLower(), out var createMethod))
            {
                return createMethod(value);
            }

            return null;
        }
    }
}
