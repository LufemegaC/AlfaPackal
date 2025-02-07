using Api_PACsServer.Models.AccessControl;
using Api_PACsServer.Repositories.IRepository.DataAccess;

namespace Api_PACsServer.Repositories.IRepository.Authentication
{
    /// <summary>
    /// Represents a repository for users.
    /// </summary>
    /// <author>Luis F. Méndez G.</author>
    public interface IUserRepository : IReadRepository<SystemUser>
    {

    }
}
