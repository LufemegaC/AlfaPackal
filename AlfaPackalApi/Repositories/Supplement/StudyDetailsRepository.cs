using AlfaPackalApi.Datos;
using Api_PACsServer.Modelos.Load;
using Api_PACsServer.Repositorio.IRepositorio.Cargas;
using Api_PACsServer.Repository.DataAccess;

namespace Api_PACsServer.Repositorio.Cargas
{
    public class StudyDetailsRepository : ReadWriteRepository<StudyDetails>, IStudyDetailsRepository
    {
        private readonly ApplicationDbContext _db;

        public StudyDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<StudyDetails> Update(StudyDetails studyLoad)
        {
            studyLoad.UpdateDate = DateTime.Now;
            _db.StudiesDetails.Update(studyLoad);
            await _db.SaveChangesAsync();
            return studyLoad;
        }
    }
}
