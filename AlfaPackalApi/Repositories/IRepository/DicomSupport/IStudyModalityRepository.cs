using Api_PACsServer.Models.DicomSupport;
using Api_PACsServer.Repositories.IRepository.DataAccess;

namespace Api_PACsServer.Repositories.IRepository.DicomSupport
{
    public interface IStudyModalityRepository: IWriteRepository<StudyModality>, IReadRepository<StudyModality>
    {
    }
}
