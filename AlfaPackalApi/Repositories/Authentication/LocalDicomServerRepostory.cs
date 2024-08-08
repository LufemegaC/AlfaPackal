using AlfaPackalApi.Datos;
using Api_PACsServer.Modelos.AccessControl;
using Api_PACsServer.Repository.DataAccess;
using Api_PACsServer.Repository.IRepository.Authentication;

namespace Api_PACsServer.Repository.Authentication
{
    public class LocalDicomServerRepostory : ReadRepository<LocalDicomServer>, ILocalDicomServerRepostory
    {

        public LocalDicomServerRepostory(ApplicationDbContext db) : base(db)
        {

        }
    }
}
