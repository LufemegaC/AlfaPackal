using Api_PACsServer.Datos;
using Api_PACsServer.Models.DicomSupport;
using Api_PACsServer.Repositories.DataAccess;
using Api_PACsServer.Repositories.IRepository.DicomSupport;

namespace Api_PACsServer.Repositories.DicomSupport
{
    public class StudyModalityRepository: ReadWriteRepository<StudyModality>, IStudyModalityRepository
    {

        private readonly ApplicationDbContext _db;

        public StudyModalityRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<StudyModality> Update(StudyModality studyModality)
        {
            //studyLoad.UpdateDate = DateTime.Now;
            _db.StudyModalities.Update(studyModality);
            await _db.SaveChangesAsync();
            return studyModality;
        }

    }
}
