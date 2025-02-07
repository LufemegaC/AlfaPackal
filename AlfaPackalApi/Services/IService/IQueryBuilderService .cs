using Api_PACsServer.Models.Dto.Base;
using Api_PACsServer.Models.Dto.DicomWeb.Qido;
using Microsoft.Data.SqlClient;

namespace Api_PACsServer.Services.IService
{
    public interface IQueryBuilderService
    {

        //string BuildSelectClause(List<string?> includeFields, bool isStudyLevel, bool isSeriesLevel, bool isInstanceLevel);
        //(string whereClause, List<SqlParameter> parameters) BuildWhereClause(object queryParamsDto);
        //string BuildOrderByClause(AdditionalParameters controlParamsDto);
        //void ValidateControlParameters(AdditionalParameters controlParams);

        QuerySpecification BuildQuerySpecification<T>(QueryRequestParameters<T> request) where T : BaseDicomQueryParametersDto;
    }
}
