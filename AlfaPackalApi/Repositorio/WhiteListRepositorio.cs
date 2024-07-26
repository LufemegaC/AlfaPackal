using AlfaPackalApi.Datos;
using Api_PACsServer.Modelos.AccessControl;
using Api_PACsServer.Repositorio.IRepositorio;
using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;

namespace Api_PACsServer.Repositorio
{
    public class WhiteListRepositorio : IWhiteListRepositorio
    {
        private readonly ApplicationDbContext _db;

        public WhiteListRepositorio(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> IsValidIP(string ip)
        {
            // Obtener el servidor DICOM con la IP proporcionada
            var server = await _db.DicomServers.FirstOrDefaultAsync(s => s.IP == ip);
            if (server == null) // Si no se encuentra el servidor, la IP no es válida
            {
                return false;
            }
            
            return await _db.WhitelistedIPs // Verificar si la ID del servidor está en la whitelist y está activa
                            .AnyAsync(w => w.DicomServerId == server.Id && w.IsActive && 
                                        (w.DateRemoved == null || w.DateRemoved > DateTime.Now));
        }

        public async Task<DicomServer> GetServerByIp(string ip)
        {
            return await _db.DicomServers.FirstOrDefaultAsync(s => s.IP == ip);   
        }

    }
}
