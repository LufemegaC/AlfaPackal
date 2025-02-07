using Api_PACsServer.Datos;
using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto.DicomWeb.Qido;
using Api_PACsServer.Models.Supplement;
using Api_PACsServer.Repositories.DataAccess;
using Api_PACsServer.Repositories.IRepository.MainEntities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Text;

namespace Api_PACsServer.Repositories.MainEntities
{
    public class SerieRepository : ReadWriteRepository<Serie>, ISerieRepository
    {
        private readonly ApplicationDbContext _db;
        public SerieRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<Serie>> ExecuteSerieQuery(QuerySpecification querySpecification)
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
                    var series = new List<Serie>();
                    while (reader.Read())
                    {
                        var study = MapToSerie(reader);
                        series.Add(study);
                    }
                    return series;
                }
            }
        }

        //// PRIVATE METHODS ////
        ///

        /// <summary>
        /// Maps data from a database reader to a Study entity.
        /// </summary>
        private Serie MapToSerie(DbDataReader reader)
        {
            return new Serie
            {
                StudyInstanceUID = reader["StudyInstanceUID"].ToString(),
                SeriesNumber = Convert.ToInt32(reader["SeriesNumber"]),
                SeriesDescription = reader["SeriesDescription"].ToString(),
                Modality = reader["Modality"].ToString(),
                BodyPartExamined = reader["BodyPartExamined"].ToString(),
                SeriesDateTime = (DateTime)reader["SeriesDateTime"],
                //ReferringPhysicianName = reader["ReferringPhysicianName"].ToString(),
            };
        }
    }
}
