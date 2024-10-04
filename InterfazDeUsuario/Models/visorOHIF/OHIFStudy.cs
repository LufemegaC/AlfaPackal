namespace InterfazDeUsuario.Models.visorOHIF
{
    public class OHIFStudy
    {
        public string StudyInstanceUID { get; set; }
        public string StudyDate { get; set; }
        public string StudyTime { get; set; }
        public string AccessionNumber { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string PatientBirthDate { get; set; }
        public string StudyDescription { get; set; }
        public List<OHIFSeries> Series { get; set; }  // Lista de series asociadas al estudio
    }
}
