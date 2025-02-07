using Api_PACsServer.Models.Dto.Base;
using Api_PACsServer.Models.Dto.DicomWeb.Qido;
using Api_PACsServer.Services.IService;
using FellowOakDicom.Network;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Data.SqlClient;
using System.Text;
using static Api_PACsServer.Utilities.QueryOperators;

namespace Api_PACsServer.Services
{
    public class QueryBuilderService : IQueryBuilderService
    {
        // Diccionario para mapear el nivel DICOM a la tabla correspondiente
        private static readonly Dictionary<DicomQueryRetrieveLevel, string> QueryLevelToTableMapping = new Dictionary<DicomQueryRetrieveLevel, string>
        {
            { DicomQueryRetrieveLevel.Patient, "Patients" },
            { DicomQueryRetrieveLevel.Study, "Studies" },
            { DicomQueryRetrieveLevel.Series, "Series" },
            { DicomQueryRetrieveLevel.Image, "Instances" },
            { DicomQueryRetrieveLevel.NotApplicable, "Studies" } // Por defecto
        };

        private void ValidateControlParameters(AdditionalParameters controlParams)
        {
            if (controlParams == null)
                throw new ArgumentException("Control parameters cannot be null.");

            if (controlParams.Page != null && int.Parse(controlParams.Page.Value) <= 0)
                throw new ArgumentException("Page number must be greater than zero.");

            if (controlParams.PageSize != null && int.Parse(controlParams.PageSize.Value) <= 0)
                throw new ArgumentException("Page size must be greater than zero.");
        }

        private string BuildControlClauses(AdditionalParameters controlParamsDto)
        {
            StringBuilder controlBuilder = new StringBuilder();

            // Ordenamiento
            if (controlParamsDto.OrderBy != null && !string.IsNullOrEmpty(controlParamsDto.OrderBy.Value))
            {
                controlBuilder.Append($" ORDER BY {controlParamsDto.OrderBy.Value}");
                if (controlParamsDto.OrderDirection != null &&
                    (controlParamsDto.OrderDirection.Value.Equals("asc", StringComparison.OrdinalIgnoreCase) ||
                     controlParamsDto.OrderDirection.Value.Equals("desc", StringComparison.OrdinalIgnoreCase)))
                {
                    controlBuilder.Append($" {controlParamsDto.OrderDirection.Value}");
                }
            }

            // Paginación
            if (controlParamsDto.PageSize != null && controlParamsDto.Page != null &&
                int.TryParse(controlParamsDto.PageSize.Value, out int pageSize) && pageSize > 0 &&
                int.TryParse(controlParamsDto.Page.Value, out int page) && page > 0)
            {
                int offset = (page - 1) * pageSize;
                controlBuilder.Append($" OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY");
            }

            return controlBuilder.ToString();
        }

        private string BuildSelectClause(List<string?> includeFields, DicomQueryRetrieveLevel level)
        {
            StringBuilder selectBuilder = new StringBuilder("SELECT ");

            // Seleccionar campos base según el nivel
            switch (level)
            {
                //case DicomQueryRetrieveLevel.Patient:
                //    // Ajustar los campos según tu tabla Patients
                //    selectBuilder.Append("PatientName");
                //    break;
                case DicomQueryRetrieveLevel.Study:
                    selectBuilder.Append("StudyInstanceUID, StudyDate, StudyTime, Modalities, InstitutionName, PatientName, StudyDescription, ReferringPhysicianName");
                    break;
                case DicomQueryRetrieveLevel.Series:
                    selectBuilder.Append("SeriesInstanceUID,SeriesNumber, SeriesDescription, Modality, BodyPartExamined, SeriesDateTime ");
                    break;
                case DicomQueryRetrieveLevel.Image:
                    selectBuilder.Append("SOPInstanceUID, SOPClassUID, InstanceNumber, ImageComments");
                    break;
                default:
                    // Caso por defecto
                    selectBuilder.Append("StudyInstanceUID, StudyDate, StudyTime, PatientName, PatientID, StudyDescription");
                    break;
            }

            // Agregar campos adicionales si se han especificado
            if (includeFields != null && includeFields.Any())
            {
                selectBuilder.Append(", " + string.Join(", ", includeFields));
            }

            return selectBuilder.ToString();
        }

        private (string whereClause, List<SqlParameter> parameters) BuildWhereClause(object queryParamsDto)
        {
            StringBuilder whereBuilder = new StringBuilder(" WHERE 1 = 1 ");
            List<SqlParameter> parameters = new List<SqlParameter>();

            foreach (var property in queryParamsDto.GetType().GetProperties())
            {
                if (property.GetValue(queryParamsDto) is QueryParameter queryParam && queryParam != null && !string.IsNullOrEmpty(queryParam.Value))
                {
                    string columnName = property.Name;
                    var formattedValue = queryParam.Value;
                    string startFormatted = null;
                    string endFormatted = null;

                    if (queryParam.DataType == QueryParameterType.DateTime && formattedValue.Contains("-"))
                    {
                        // Manejo de rangos para fechas
                        var rangeValues = formattedValue.Split('-');
                        if (rangeValues.Length == 2 &&
                            DateTime.TryParseExact(rangeValues[0].Trim(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var startDate) &&
                            DateTime.TryParseExact(rangeValues[1].Trim(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var endDate))
                        {
                            startFormatted = startDate.ToString("yyyy-MM-dd");
                            endFormatted = endDate.ToString("yyyy-MM-dd");
                        }
                    }

                    // Construir la cláusula según el operador SQL
                    switch (queryParam.Operator)
                    {
                        case SqlOperator.Equals:
                            whereBuilder.Append($" AND {columnName} = @{columnName}");
                            parameters.Add(new SqlParameter($"@{columnName}", formattedValue));
                            break;
                        case SqlOperator.GreaterThan:
                            whereBuilder.Append($" AND {columnName} > @{columnName}");
                            parameters.Add(new SqlParameter($"@{columnName}", formattedValue));
                            break;
                        case SqlOperator.LessThan:
                            whereBuilder.Append($" AND {columnName} < @{columnName}");
                            parameters.Add(new SqlParameter($"@{columnName}", formattedValue));
                            break;
                        case SqlOperator.GreaterThanOrEqual:
                            whereBuilder.Append($" AND {columnName} >= @{columnName}");
                            parameters.Add(new SqlParameter($"@{columnName}", formattedValue));
                            break;
                        case SqlOperator.LessThanOrEqual:
                            whereBuilder.Append($" AND {columnName} <= @{columnName}");
                            parameters.Add(new SqlParameter($"@{columnName}", formattedValue));
                            break;
                        case SqlOperator.NotEqual:
                            whereBuilder.Append($" AND {columnName} != @{columnName}");
                            parameters.Add(new SqlParameter($"@{columnName}", formattedValue));
                            break;
                        case SqlOperator.Between:
                            if (!string.IsNullOrEmpty(startFormatted) && !string.IsNullOrEmpty(endFormatted))
                            {
                                whereBuilder.Append($" AND {columnName} BETWEEN @Start{columnName} AND @End{columnName}");
                                parameters.Add(new SqlParameter($"@Start{columnName}", startFormatted));
                                parameters.Add(new SqlParameter($"@End{columnName}", endFormatted));
                            }
                            break;
                        case SqlOperator.Like:
                            var likeValue = $"%{formattedValue}%";
                            whereBuilder.Append($" AND {columnName} LIKE @param_{columnName}");
                            parameters.Add(new SqlParameter($"@param_{columnName}", likeValue));
                            break;
                    }
                }
            }
            return (whereBuilder.ToString(), parameters);
        }

        public QuerySpecification BuildQuerySpecification<T>(QueryRequestParameters<T> request) where T : BaseDicomQueryParametersDto
        {
            // Obtener el nivel DICOM del request
            var level = request.DicomParameters.DicomQueryLevel;

            // Obtener la tabla a partir del nivel
            string tableName = QueryLevelToTableMapping.ContainsKey(level)
                ? QueryLevelToTableMapping[level]
                : "Studies"; // Por defecto


            // Validar include fields
            var includeFieldsSpecified = (request.DicomParameters.IncludeFields.Count > 0);

            // SELECT
            var selectClause = BuildSelectClause(request.DicomParameters.IncludeFields, level);

            // FROM
            string fromClause = $" FROM {tableName}"; 

            // WHERE
            var (whereClause, parameters) = BuildWhereClause(request.DicomParameters);

            // Cláusulas de control (ORDER BY, paginación, etc.)
            var controlClauses = BuildControlClauses(request.AdditionalParameters);

            return new QuerySpecification(selectClause, fromClause, whereClause, controlClauses, parameters, includeFieldsSpecified);

        }
    }
}
