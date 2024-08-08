using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.AccessControl;
using Api_PACsServer.Models.Dto.AuthDtos;
using Api_PACsServer.Repository.IRepository.RepositoryBase;

namespace Api_PACsServer.Repository.IRepository.Authentication
{
    /// <summary>
    /// Represents a repository for users.
    /// </summary>
    /// <author>Luis F. Méndez G.</author>
    public interface IUserRepository : IReadRepository<SystemUser>
    {

    }
}
