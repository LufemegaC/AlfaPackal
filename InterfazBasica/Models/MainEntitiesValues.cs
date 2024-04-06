using FellowOakDicom.Imaging;

namespace InterfazBasica_DCStore.Models
{
    public class MainEntitiesValues
    {
        // ** Main propierties
        // Patient
        public string? PatientID { get; set; }
        public string? IssuerOfPatientID { get; set; }
        public string? GeneratedPatienteID { get; set; }
        public bool ExistPatient { get; set; } = false;
        public int PACS_PatientID { get; set; }
        // Study
        public string? StudyInstanceUID { get; set; }
        public bool ExistStudy { get; set; } = false;
        public int PACS_EstudioID { get; set; }
        // Serie
        public string? SeriesInstanceUID { get; set; }
        public bool ExistSerie { get; set; } = false;
        public int PACS_SerieID { get; set; }
        // Image
        public string? SOPInstanceUID { get; set; }
        public bool ExistImage { get; set; } = false;
        public int PACS_ImagenID { get; set; }
        public DicomImage? DicomImage { get; set; }

    }
}

