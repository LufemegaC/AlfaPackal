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
        public async Task<Imagen> Actualizar(Imagen entidad)
        {
            //entidad.FechaActualizacion = DateTime.Now; 
            _db.Imagenes.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
        public async Task<bool> ExisteImagenInstanceUID(string imageInstanceUID)
        {
            return await _db.Imagenes.AnyAsync(e => e.SOPInstanceUID == imageInstanceUID);
        }

        public async Task<Imagen> GetImageByInstanceUID(string imageInstanceUID)
        {
            return await _db.Imagenes.FirstOrDefaultAsync(e => e.SOPInstanceUID == imageInstanceUID);
        }

    }
}
