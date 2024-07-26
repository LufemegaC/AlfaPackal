using AlfaPackalApi.Datos;
using Api_PACsServer.Modelos.Cargas;
using Api_PACsServer.Repositorio.IRepositorio.Cargas;
using Microsoft.EntityFrameworkCore;

namespace Api_PACsServer.Repositorio.Cargas
{
    public class ImagenCargaRepositorio: RepositorioLecturaYEscritura<ImagenCarga>, IImagenCargaRepositorio
    {
        private readonly ApplicationDbContext _db;
        public ImagenCargaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ImagenCarga> GetByPacsImageId(int pacsImagenID)
        {
            return await _db.ImagenesCarga
                .Include(ic => ic.Imagen)
                .FirstOrDefaultAsync(ic => ic.PACS_ImagenID == pacsImagenID);
        }

        //public async Task<ImagenCarga> Actualizar(ImagenCarga entidad)
        //{
        //    entidad.UpdateDate = DateTime.Now;
        //    _db.ImagenesCarga.Update(entidad);
        //    await _db.SaveChangesAsync();
        //    return entidad;
        //}
    }
}
