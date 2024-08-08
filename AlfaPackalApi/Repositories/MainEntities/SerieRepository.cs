using AlfaPackalApi.Datos;
using AlfaPackalApi.Modelos;
using Api_PACsServer.Repositorio.IRepositorio.Pacs;
using Api_PACsServer.Repository.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Api_PACsServer.Repositorio.Pacs
{
    public class SerieRepository : ReadWriteRepository<Serie>, ISerieRepository
    {
        private readonly ApplicationDbContext _db;
        public SerieRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
