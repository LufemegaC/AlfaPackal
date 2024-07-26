using Api_PACsServer.Modelos.AccessControl;
using Api_PACsServer.Modelos.Dto;

namespace Api_PACsServer.Repositorio.IRepositorio
{
    public interface IWhiteListRepositorio
    {
        Task<bool> IsValidIP(string ip);

        Task<DicomServer> GetServerByIp(string ip);
    }
}
