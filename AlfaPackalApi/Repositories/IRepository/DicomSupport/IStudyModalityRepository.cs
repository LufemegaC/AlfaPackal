using Api_PACsServer.Models.DicomList;
using Api_PACsServer.Repository.IRepository.RepositoryBase;

namespace Api_PACsServer.Repositories.IRepository.DicomSupport
{
    public interface IStudyModalityRepository: IWriteRepository<StudyModality>, IReadRepository<StudyModality>
    {
    }
}
