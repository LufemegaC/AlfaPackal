using AlfaPackalApi.Datos;
using Microsoft.EntityFrameworkCore;
using Api_PACsServer.Repositorio.IRepositorio.Pacs;
using Api_PACsServer.Repository.DataAccess;
using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Models.Dto.Studies;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Data.SqlClient;
using System.Text;
using Api_PACsServer.Modelos.Load;
using Api_PACsServer.Models.DicomList;
using static Api_PACsServer.Utilities.QueryOperators;

namespace Api_PACsServer.Repositorio.Pacs
{
    public class StudyRepository : ReadWriteRepository<Study>, IStudyRepository
    {
        private readonly ApplicationDbContext _db;
        private int _pageSize; // Number of records for each page

        public StudyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
            _pageSize = 15; 
        }

        public PagedList<Study> GetStudies(int pageNumber, int pageSize)
        {     
            IQueryable<Study> query = _db.Studies
            //.Where(study => study.InstitutionID == parameters.InstitutionId)
            .OrderByDescending(study => study.StudyDate);

            // Get the total count of records that match the query
            var count = query.Count();
            // Apply pagination to the query
            var items = query.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToList();
            // Return the paginated list of studies
            return new PagedList<Study>(items, count, pageNumber, pageSize);
        }

        /// <summary>
        /// Executes a dynamic QIDO query based on the control and study parameters provided.
        /// Constructs a dynamic SQL query to retrieve data from the Studies and StudyDetails tables.
        /// </summary>
        /// <param name="controlParams">Control parameters including pagination, ordering, and inclusion fields.</param>
        /// <param name="studyParams">Study parameters for filtering the search.</param>
        /// <returns>A tuple containing a list of Study and StudyDetails.</returns>
        public (List<Study>, List<StudyDetails>) ExecuteDynamicQidoQuery(ControlQueryParametersDto controlParams, StudyQueryParametersDto studyParams)
        {
            // Initialize the StringBuilder for the SQL query.
            StringBuilder queryBuilder = new StringBuilder("SELECT ");

            // 1. Dynamic field selection (IncludeFields).
            // Always include base fields, and add additional fields if specified in IncludeFields.
            queryBuilder.Append("s.StudyInstanceUID, s.StudyDate, s.StudyTime, sd.AccessionNumber, s.PatientName, s.PatientID, s.StudyDescription, sd.NumberOfStudyRelatedSeries, sd.NumberOfStudyRelatedInstances, s.ReferringPhysicianName");
            if (controlParams.IncludeFields != null && controlParams.IncludeFields.Any())
            {
                queryBuilder.Append(", " + string.Join(", ", controlParams.IncludeFields));
            }

            // Adding FROM clause to query, joining Study and StudyDetails tables.
            queryBuilder.Append(" FROM Studies AS s LEFT JOIN StudyDetails AS sd ON s.StudyInstanceUID = sd.StudyInstanceUID WHERE 1 = 1 ");

            // 2. Dynamic WHERE clause creation for metadata and control parameters.
            var parameters = new List<SqlParameter>();

            foreach (var property in studyParams.GetType().GetProperties())
            {
                if (property.GetValue(studyParams) is QueryParameter queryParam && queryParam != null && !string.IsNullOrEmpty(queryParam.Value))
                {
                    string columnName = property.Name;
                    switch (queryParam.Operator)
                    {
                        case SqlOperator.Equals:
                            queryBuilder.Append($" AND {columnName} = @{columnName}");
                            parameters.Add(new SqlParameter($"@{columnName}", queryParam.Value));
                            break;
                        case SqlOperator.GreaterThan:
                            queryBuilder.Append($" AND {columnName} > @{columnName}");
                            parameters.Add(new SqlParameter($"@{columnName}", queryParam.Value));
                            break;
                        case SqlOperator.LessThan:
                            queryBuilder.Append($" AND {columnName} < @{columnName}");
                            parameters.Add(new SqlParameter($"@{columnName}", queryParam.Value));
                            break;
                        case SqlOperator.GreaterThanOrEqual:
                            queryBuilder.Append($" AND {columnName} >= @{columnName}");
                            parameters.Add(new SqlParameter($"@{columnName}", queryParam.Value));
                            break;
                        case SqlOperator.LessThanOrEqual:
                            queryBuilder.Append($" AND {columnName} <= @{columnName}");
                            parameters.Add(new SqlParameter($"@{columnName}", queryParam.Value));
                            break;
                        case SqlOperator.NotEqual:
                            queryBuilder.Append($" AND {columnName} != @{columnName}");
                            parameters.Add(new SqlParameter($"@{columnName}", queryParam.Value));
                            break;
                        case SqlOperator.Between:
                            var rangeValues = queryParam.Value.Split('-');
                            if (rangeValues.Length == 2)
                            {
                                queryBuilder.Append($" AND {columnName} BETWEEN @Start{columnName} AND @End{columnName}");
                                parameters.Add(new SqlParameter($"@Start{columnName}", rangeValues[0].Trim()));
                                parameters.Add(new SqlParameter($"@End{columnName}", rangeValues[1].Trim()));
                            }
                            break;
                        case SqlOperator.Like:
                            queryBuilder.Append($" AND {columnName} LIKE @{columnName}");
                            parameters.Add(new SqlParameter($"@{columnName}", queryParam.Value.Replace("*", "%")));
                            break;
                    }
                }
            }

            // 3. Applying order and pagination parameters.
            if (controlParams.OrderBy != null && !string.IsNullOrEmpty(controlParams.OrderBy.Value))
            {
                queryBuilder.Append($" ORDER BY {controlParams.OrderBy.Value}");
                if (controlParams.OrderDirection != null && (controlParams.OrderDirection.Value.ToLower() == "asc" || controlParams.OrderDirection.Value.ToLower() == "desc"))
                {
                    queryBuilder.Append($" {controlParams.OrderDirection.Value}");
                }
            }
            else
            {
                // Default ordering if none is specified.
                queryBuilder.Append(" ORDER BY s.StudyDate DESC");
            }

            if (controlParams.PageSize != null && controlParams.Page != null &&
                int.TryParse(controlParams.PageSize.Value, out int pageSize) && pageSize > 0 &&
                int.TryParse(controlParams.Page.Value, out int page) && page > 0)
            {
                int offset = (page - 1) * pageSize;
                queryBuilder.Append(" OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");
                parameters.Add(new SqlParameter("@Offset", offset));
                parameters.Add(new SqlParameter("@PageSize", pageSize));
            }

            // 4. Execute the query using the ApplicationDbContext.
            using (var command = _db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = queryBuilder.ToString();
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddRange(parameters.ToArray());

                _db.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                {
                    var studies = new List<Study>();
                    var studyDetailsList = new List<StudyDetails>();
                    while (reader.Read())
                    {
                        // Map the results to the Study model.
                        var study = new Study
                        {
                            StudyInstanceUID = reader["StudyInstanceUID"].ToString(),
                            StudyDate = (DateTime)reader["StudyDate"],
                            StudyTime = (TimeSpan)reader["StudyTime"],
                            PatientName = reader["PatientName"].ToString(),
                            PatientID = reader["PatientID"].ToString(),
                            StudyDescription = reader["StudyDescription"].ToString(),
                            ReferringPhysicianName = reader["ReferringPhysicianName"].ToString(),
                            ModalitiesInStudy = GetStudyModalities(reader["StudyID"].ToString())
                        };

                        studies.Add(study);

                        // Map the results to the StudyDetails model.
                        var studyDetails = new StudyDetails
                        {
                            StudyInstanceUID = reader["StudyInstanceUID"].ToString(),
                            AccessionNumber = reader["AccessionNumber"].ToString(),
                            NumberOfStudyRelatedSeries = Convert.ToInt32(reader["NumberOfStudyRelatedSeries"]),
                            NumberOfStudyRelatedInstances = Convert.ToInt32(reader["NumberOfStudyRelatedInstances"])
                        };
                        studyDetailsList.Add(studyDetails);
                    }
                    return (studies, studyDetailsList);
                }
            }
        }


        /// <summary>
        /// Retrieves all modalities for a given study.
        /// </summary>
        /// <param name="studyID">The unique identifier for the study.</param>
        /// <returns>A collection of StudyModality objects associated with the study.</returns>
        private ICollection<StudyModality> GetStudyModalities(string studyID)
        {
            var modalities = new List<StudyModality>();

            using (var modalityCommand = _db.Database.GetDbConnection().CreateCommand())
            {
                modalityCommand.CommandText = "SELECT Modality FROM StudyModalities WHERE StudyID = @StudyID";
                modalityCommand.Parameters.Add(new SqlParameter("@StudyID", studyID));
                modalityCommand.CommandType = System.Data.CommandType.Text;

                _db.Database.OpenConnection();

                using (var modalityReader = modalityCommand.ExecuteReader())
                {
                    while (modalityReader.Read())
                    {
                        modalities.Add(new StudyModality(int.Parse(studyID), modalityReader["Modality"].ToString()));
                    }
                }
            }

            return modalities;
        }
    }
}
