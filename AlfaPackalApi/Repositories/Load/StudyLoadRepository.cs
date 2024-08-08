using AlfaPackalApi.Datos;
using Api_PACsServer.Modelos.Load;
using Api_PACsServer.Repositorio.IRepositorio.Cargas;
using Api_PACsServer.Repository.DataAccess;

namespace Api_PACsServer.Repositorio.Cargas
{
    public class StudyLoadRepository : ReadWriteRepository<StudyLoad>, IStudyLoadRepository
    {
        private readonly ApplicationDbContext _db;

        public StudyLoadRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<StudyLoad> Update(StudyLoad studyLoad)
        {
            studyLoad.UpdateDate = DateTime.Now;
            _db.StudiesLoad.Update(studyLoad);
            await _db.SaveChangesAsync();
            return studyLoad;
        }
    }
}
