using FellowOakDicom.Imaging;

namespace Api_PACsServer.Modelos
{
    public class MainEntitiesValues
    {
        // ** Main propierties
        // Patient
        public string? PatientID { get; set; }
        public string? IssuerOfPatientID { get; set; }
        public string? GeneratedPatientID { get; set; }
        public bool ExistPatient { get; set; }
        public int PACS_PatientID { get; set; }
        // Study
        public string? StudyInstanceUID { get; set; }
        public bool ExistStudy { get; set; }
        public int PACS_EstudioID { get; set; }
        // Serie
        public string? SeriesInstanceUID { get; set; }
        public bool ExistSerie { get; set; }
        public int PACS_SerieID { get; set; }
        // Image
        public string? SOPInstanceUID { get; set; }
        public bool ExistImage { get; set; }
        public int PACS_ImagenID { get; set; }
        public DicomImage? DicomImage { get; set; }

    }
}
