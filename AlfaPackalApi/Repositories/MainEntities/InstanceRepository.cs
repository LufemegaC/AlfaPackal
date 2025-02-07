using Api_PACsServer.Datos;
using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto.DicomWeb.Qido;
using Api_PACsServer.Repositories.DataAccess;
using Api_PACsServer.Repositories.IRepository.MainEntities;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Text;

namespace Api_PACsServer.Repositories.MainEntities
{
    public class InstanceRepository : ReadWriteRepository<Instance>, IInstanceRepository
    {
        private readonly ApplicationDbContext _db;
        public InstanceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<Instance>> ExecuteInstanceQuery(QuerySpecification querySpecification)
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
                    var Instances = new List<Instance>();
                    while (reader.Read())
                    {
                        var Instance = MapToInstance(reader);
                        Instances.Add(Instance);
                    }
                    return Instances;
                }
            }
        }

        //// PRIVATE METHODS ////
        ///

        /// <summary>
        /// Maps data from a database reader to a Study entity.
        /// </summary>
        private Instance MapToInstance(DbDataReader reader)
        {
            return new Instance
            {
                SOPInstanceUID = reader["SOPInstanceUID"].ToString(),
                SOPClassUID = reader["SOPClassUID"].ToString(),
                InstanceNumber = Convert.ToInt32(reader["InstanceNumber"]),
                ImageComments = reader["SOPInstanceUID"].ToString()
            };
        }
    }
}
