using Api_PACsServer.Utilities;
using static Api_PACsServer.Utilities.QueryOperators;

namespace Api_PACsServer.Models.Dto.DicomWeb.Qido
{
    public class QueryParameter
    {
        public QueryParameter(string value, SqlOperator sqlOperator, QueryParameterType dataType, QueryParameterCategory category)
        {
            Value = value;
            Operator = sqlOperator;
            DataType = dataType;
            Category = category;
        }


        public string? Value { get; set; }
        public SqlOperator Operator { get; set; }

        public QueryParameterType DataType { get; set; }

        public QueryParameterCategory Category { get; set; }


        // Static methods to create control parameters

        /// <summary>
        /// Creates a QueryParameter for the Limit control parameter.
        /// </summary>
        /// <param name="value">The limit value.</param>
        /// <returns>An instance of QueryParameter configured for the Limit parameter.</returns>
        public static QueryParameter CreateLimitParameter(string value)
        {
            return new QueryParameter(value, SqlOperator.Equals, QueryParameterType.Int, QueryParameterCategory.Additional);
        }

        /// <summary>
        /// Creates a QueryParameter for the OrderBy control parameter.
        /// </summary>
        /// <param name="value">The order by value.</param>
        /// <returns>An instance of QueryParameter configured for the OrderBy parameter.</returns>
        public static QueryParameter CreateOrderByParameter(string value)
        {
            return new QueryParameter(value, SqlOperator.Equals, QueryParameterType.String, QueryParameterCategory.Additional);
        }

        /// <summary>
        /// Creates a QueryParameter for the OrderDirection control parameter.
        /// </summary>
        /// <param name="value">The order direction value (e.g., "ASC" or "DESC").</param>
        /// <returns>An instance of QueryParameter configured for the OrderDirection parameter.</returns>
        public static QueryParameter CreateOrderDirectionParameter(string value)
        {
            return new QueryParameter(value, SqlOperator.Equals, QueryParameterType.String, QueryParameterCategory.Additional);
        }

        /// <summary>
        /// Creates a QueryParameter for the Offset control parameter.
        /// </summary>
        /// <param name="value">The offset value.</param>
        /// <returns>An instance of QueryParameter configured for the Offset parameter.</returns>
        public static QueryParameter CreateOffsetParameter(string value)
        {
            return new QueryParameter(value, SqlOperator.Equals, QueryParameterType.Int, QueryParameterCategory.Additional);
        }

        /// <summary>
        /// Creates a QueryParameter for the Format control parameter.
        /// </summary>
        /// <param name="value">The format value (e.g., "json" or "dicom").</param>
        /// <returns>An instance of QueryParameter configured for the Format parameter.</returns>
        public static QueryParameter CreateFormatParameter(string value)
        {
            return new QueryParameter(value, SqlOperator.Equals, QueryParameterType.String, QueryParameterCategory.Additional);
        }

        /// <summary>
        /// Creates a QueryParameter for the Page control parameter.
        /// </summary>
        /// <param name="value">The page number value.</param>
        /// <returns>An instance of QueryParameter configured for the Page parameter.</returns>
        public static QueryParameter CreatePageParameter(string value)
        {
            return new QueryParameter(value, SqlOperator.Equals, QueryParameterType.Int, QueryParameterCategory.Additional);
        }

        /// <summary>
        /// Creates a QueryParameter for the PageSize control parameter.
        /// </summary>
        /// <param name="value">The page size value.</param>
        /// <returns>An instance of QueryParameter configured for the PageSize parameter.</returns>
        public static QueryParameter CreatePageSizeParameter(string value)
        {
            return new QueryParameter(value, SqlOperator.Equals, QueryParameterType.Int, QueryParameterCategory.Additional);
        }

        // -- Static methods to create Dicom parameters

        /// <summary>
        /// Creates a QueryParameter for general string fields like PatientName or InstitutionName.
        /// Converts the input value to uppercase and removes wildcard characters (*) if present.
        /// </summary>
        /// <param name="value">The input string value.</param>
        /// <returns>An instance of QueryParameter configured for string fields with LIKE operator.</returns>
        public static QueryParameter CreateSearchableStringParameter(string value)
        {
            // Convert the value to uppercase
            value = value.ToUpper();

            // Remove wildcard characters from the value
            value = value.Replace("*", "");

            return new QueryParameter(value, SqlOperator.Like, QueryParameterType.String, QueryParameterCategory.Dicom);
        }

        /// <summary>
        /// Creates a QueryParameter for categorical string fields like PatientSex or PatientPosition.
        /// Uses the equality operator as these fields have finite, well-defined values.
        /// </summary>
        /// <param name="value">The input string value.</param>
        /// <returns>An instance of QueryParameter configured for categorical string fields with EQUALS operator.</returns>
        public static QueryParameter CreateExactStringParameter(string value)
        {
            // Convert the value to uppercase
            value = value.ToUpper();

            return new QueryParameter(value, SqlOperator.Equals, QueryParameterType.String, QueryParameterCategory.Dicom);
        }




    }
}
