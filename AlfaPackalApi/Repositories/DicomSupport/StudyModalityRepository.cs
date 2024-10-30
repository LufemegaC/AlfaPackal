using AlfaPackalApi.Datos;
using Api_PACsServer.Modelos.Load;
using Api_PACsServer.Models.DicomList;
using Api_PACsServer.Repositories.IRepository.DicomSupport;
using Api_PACsServer.Repository.DataAccess;

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
