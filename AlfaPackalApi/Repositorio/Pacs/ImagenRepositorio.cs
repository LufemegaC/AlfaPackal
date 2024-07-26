using AlfaPackalApi.Datos;
using AlfaPackalApi.Modelos;
using Api_PACsServer.Repositorio.IRepositorio.Pacs;
using Microsoft.EntityFrameworkCore;


namespace Api_PACsServer.Repositorio.Pacs
{
    public class ImagenRepositorio : RepositorioLecturaYEscritura<Imagen>, IImagenRepositorio
    {
        private readonly ApplicationDbContext _db;
        public ImagenRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<bool> ExistBySOPInstanceUID(string sopInstanceUID)
        {
            return await _db.Imagenes.AnyAsync(e => e.SOPInstanceUID == sopInstanceUID);
        }

        public async Task<Imagen> GetByInstanceUID(string sopInstanceUID)
        {
            return await _db.Imagenes.FirstOrDefaultAsync(e => e.SOPInstanceUID == sopInstanceUID);
        }

    }
}
