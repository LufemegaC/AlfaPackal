using AlfaPackalApi.Datos;
using Api_PACsServer.Modelos.Cargas;
using Api_PACsServer.Repositorio.IRepositorio.Cargas;
using Microsoft.EntityFrameworkCore;

namespace Api_PACsServer.Repositorio.Cargas
{
    public class SerieCargaRepositorio : RepositorioLecturaYEscritura<SerieCarga>, ISerieCargaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public SerieCargaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<SerieCarga> UpdateSizeForNImage(SerieCarga serieCarga, decimal fileSizeMb)
        {
            serieCarga.TotalFileSizeMB += fileSizeMb;
            serieCarga.NumberOfImages += 1;
            serieCarga.UpdateDate = DateTime.Now;
            _db.SeriesCarga.Update(serieCarga);
            await _db.SaveChangesAsync(); // Guardar los cambios en la base de datos
            return serieCarga;
        }

        public async Task<SerieCarga> GetByImagenCargaId(int imageCargaId)
        {
            // Buscar la entidad ImagenCarga asociada al PACS_ImagenCargaID
            var imagenCarga = await _db.ImagenesCarga
                .FirstOrDefaultAsync(ic => ic.PACS_ImagenCargaID == imageCargaId);

            if (imagenCarga == null)
            {
                // Manejar el caso donde no se encuentra la entidad ImagenCarga
                return null;
            }

            // Buscar la entidad SerieCarga asociada al PACS_SerieID de la ImagenCarga
            var serieCarga = await _db.SeriesCarga
                .FirstOrDefaultAsync(sc => sc.PACS_SerieID == imagenCarga.PACS_ImagenID);

            if (serieCarga == null)
            {
                // Manejar el caso donde no se encuentra la entidad SerieCarga
                return null;
            }
            return serieCarga;
        }


    }
}
