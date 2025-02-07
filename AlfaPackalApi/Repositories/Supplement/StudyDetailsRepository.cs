using Api_PACsServer.Datos;
using Api_PACsServer.Models.Supplement;
using Api_PACsServer.Repositories.DataAccess;
using Api_PACsServer.Repositories.IRepository.Supplement;
using Microsoft.EntityFrameworkCore;

namespace Api_PACsServer.Repositories.Supplement
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
            _db.StudyDetails.Update(studyLoad);
            await _db.SaveChangesAsync();
            return studyLoad;
        }

        public async Task<List<StudyDetails>> GetDetailsByUIDs(List<string> studyInstanceUIDs)
        {
            return await _db.StudyDetails
                .Where(d => studyInstanceUIDs.Contains(d.StudyInstanceUID))
                .ToListAsync();
        }
    }
}
