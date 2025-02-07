namespace Api_PACsServer.Utilities
{
    public static class QueryOperators
    {
        public enum SqlOperator
        {
            Equals,
            GreaterThan,
            LessThan,
            GreaterThanOrEqual,
            LessThanOrEqual,
            NotEqual,
            Like,
            Between,
            OrderBy,
            Descending,
            Ascending
        }

        public enum QueryParameterType
        {
            String,
            Int,
            DateTime,
            Decimal,
            Boolean
        }

        public enum QueryParameterCategory
        {
            Additional,
            Dicom
        }



    }
}
