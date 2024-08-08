using AlfaPackalApi.Datos;
using Api_PACsServer.Modelos.AccessControl;
using Api_PACsServer.Repository.DataAccess;
using Api_PACsServer.Repository.IRepository.Authentication;


namespace Api_PACsServer.Repository.Authentication
{
    public class InstitutionRespository: ReadRepository<Institution>,  IInstitutionRespository
    {

        public InstitutionRespository(ApplicationDbContext db) : base(db)
        {

        }

    }
}
