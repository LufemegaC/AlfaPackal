using InterfazBasica_DCStore.Service.IDicomService;

namespace InterfazBasica_DCStore.Service.DicomServices
{
    public class DicomImageFinderService : IDicomImageFinderService
    {
        public List<string> FindFilesByUID(string PatientId, string StudyUID, string SeriesUID)
        {
            throw new NotImplementedException();
        }

        public List<string> FindPatientFiles(string PatientName, string PatientId)
        {
            throw new NotImplementedException();
        }

        public List<string> FindSeriesFiles(string PatientName, string PatientId, string AccessionNbr, string StudyUID, string SeriesUID, string Modality)
        {
            throw new NotImplementedException();
        }

        public List<string> FindStudyFiles(string PatientName, string PatientId, string AccessionNbr, string StudyUID)
        {
            throw new NotImplementedException();
        }
    }
}
