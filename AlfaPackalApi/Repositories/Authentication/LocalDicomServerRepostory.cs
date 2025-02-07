using Api_PACsServer.Datos;
using Api_PACsServer.Models.AccessControl;
using Api_PACsServer.Repositories.DataAccess;
using Api_PACsServer.Repositories.IRepository.Authentication;

namespace Api_PACsServer.Repositories.Authentication
{
    public class LocalDicomServerRepostory : ReadRepository<LocalDicomServer>, ILocalDicomServerRepostory
    {

        public LocalDicomServerRepostory(ApplicationDbContext db) : base(db)
        {

        }
    }
}
