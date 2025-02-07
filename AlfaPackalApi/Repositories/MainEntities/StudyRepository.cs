using Api_PACsServer.Datos;
using Api_PACsServer.Models;
using Api_PACsServer.Repositories.DataAccess;
using Api_PACsServer.Repositories.IRepository.MainEntities;
using Api_PACsServer.Models.Specifications;
using Api_PACsServer.Models.Supplement;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Api_PACsServer.Models.Dto.DicomWeb.Qido;
using System.Data.Common;
using System.Reflection.PortableExecutable;

namespace Api_PACsServer.Repositories.MainEntities
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

        public async Task<List<Study>> ExecuteStudyQuery(QuerySpecification querySpecification)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append(querySpecification.SelectClause);
            queryBuilder.Append(querySpecification.FromClause);
            queryBuilder.Append(querySpecification.WhereClause);
            queryBuilder.Append(querySpecification.AdditionalClause);

            using (var command = _db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = queryBuilder.ToString();
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddRange(querySpecification.Parameters.ToArray());

                _db.Database.OpenConnection();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var studies = new List<Study>();
                    while (reader.Read())
                    {
                        var study = MapToStudy(reader);
                        studies.Add(study);
                    }
                    return studies;
                }
            }
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


        /// PRIVATE METHODS ///
        
        /// <summary>
        /// Maps data from a database reader to a Study entity.
        /// </summary>
        private Study MapToStudy(DbDataReader reader)
        {
            return new Study
            {
                StudyInstanceUID = reader["StudyInstanceUID"].ToString(),
                StudyDate = (DateTime)reader["StudyDate"],
                StudyTime = reader["StudyTime"] as TimeSpan?,
                PatientName = reader["PatientName"].ToString(),
                PatientID = reader["PatientID"].ToString(),
                StudyDescription = reader["StudyDescription"].ToString(),
                ReferringPhysicianName = reader["ReferringPhysicianName"].ToString(),
            };
        }
    }
}
