using AlfaPackalApi.Datos;
using Api_PACsServer.Modelos.Load;
using Api_PACsServer.Repositorio.IRepositorio.Cargas;
using Api_PACsServer.Repository.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Api_PACsServer.Repositorio.Cargas
{
    public class InstanceDetailsRepository: ReadWriteRepository<InstanceDetails>, IInstanceDetailsRepository
    {
        private readonly ApplicationDbContext _db;
        public InstanceDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
