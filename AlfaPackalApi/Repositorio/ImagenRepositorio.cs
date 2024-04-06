using AlfaPackalApi.Datos;
using AlfaPackalApi.Modelos;
using AlfaPackalApi.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AlfaPackalApi.Repositorio
{
    public class ImagenRepositorio : Repositorio<Imagen>, IImagenRepositorio
    {
        private readonly ApplicationDbContext _db;
        public ImagenRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Imagen> GetBySOPInstanceUID(string sopInstanceUID)
        {
            return await _db.Imagenes.FirstOrDefaultAsync(e => e.SOPInstanceUID == sopInstanceUID);
        }

        public async Task<bool> ExistBySOPInstanceUID(string sopInstanceUID)
        {
            return await _db.Imagenes.AnyAsync(e => e.SOPInstanceUID == sopInstanceUID);
        }

    }
}
