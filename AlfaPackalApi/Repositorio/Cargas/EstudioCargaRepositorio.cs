using AlfaPackalApi.Datos;
using Api_PACsServer.Modelos.Cargas;
using Api_PACsServer.Repositorio.IRepositorio;
using Api_PACsServer.Repositorio.IRepositorio.Cargas;
using Microsoft.EntityFrameworkCore;

namespace Api_PACsServer.Repositorio.Cargas
{
    public class EstudioCargaRepositorio : RepositorioLecturaYEscritura<EstudioCarga>, IEstudioCargaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public EstudioCargaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<EstudioCarga> UpdateSizeForNImage(EstudioCarga estudioCarga, decimal fileSizeMb)
        {
            estudioCarga.TotalFileSizeMB += fileSizeMb;
            estudioCarga.NumberOfFiles += 1;
            estudioCarga.UpdateDate = DateTime.Now;
            _db.EstudiosCarga.Update(estudioCarga);
            await _db.SaveChangesAsync(); // Guardar los cambios en la base de datos
            return estudioCarga;
        }

        public async Task<EstudioCarga> UpdateSizeForForNSerie(EstudioCarga estudioCarga)
        {
            estudioCarga.NumberOfSeries += 1;
            estudioCarga.UpdateDate = DateTime.Now;
            _db.EstudiosCarga.Update(estudioCarga);
            await _db.SaveChangesAsync(); // Guardar los cambios en la base de datos
            return estudioCarga;
        }

        public async Task<EstudioCarga> GetByImageCargaId(int imagenCargaId)
        {
            // Buscar la entidad ImagenCarga asociada al PACS_ImagenCargaID
            var imagenCarga = await _db.ImagenesCarga
                .FirstOrDefaultAsync(ic => ic.PACS_ImagenCargaID == imagenCargaId);

            if (imagenCarga == null)
            {
                // Manejar el caso donde no se encuentra la entidad ImagenCarga
                return null;
            }

            // Buscar la entidad EstudioCarga asociada al PACS_EstudioID de la ImagenCarga
            var estudioCarga = await _db.EstudiosCarga
                .FirstOrDefaultAsync(ec => ec.PACS_EstudioID == imagenCarga.PACS_ImagenID);

            if (estudioCarga == null)
            {
                // Manejar el caso donde no se encuentra la entidad EstudioCarga
                return null;
            }
            return estudioCarga;
        }

        public async Task<EstudioCarga> GetBySerieCargaId(int serieCargaId)
        {
            // Buscar la entidad SerieCarga asociada al PACS_SerieID
            var serieCarga = await _db.SeriesCarga
                .FirstOrDefaultAsync(sc => sc.PACS_SerieCargaID == serieCargaId);
            if (serieCarga == null)
            {
                // Manejar el caso donde no se encuentra la entidad SerieCarga
                return null;
            }
            // Buscar la entidad EstudioCarga asociada al PACS_EstudioID de la SerieCarga
            var estudioCarga = await _db.EstudiosCarga
                .FirstOrDefaultAsync(ec => ec.PACS_EstudioID == serieCarga.PACS_SerieID);
            if (estudioCarga == null)
            {
                // Manejar el caso donde no se encuentra la entidad EstudioCarga
                return null;
            }
            return estudioCarga;
        }


    }
}
