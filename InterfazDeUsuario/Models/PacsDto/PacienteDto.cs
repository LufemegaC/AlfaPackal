namespace InterfazDeUsuario.Models.Pacs
{
    public class PacienteDto
    {
        // ID del paciente
        public string? PatientID { get; set; } // ID del paciente DICOM/Metadato
        public string GeneratedPatientID { get; set; }
        // Nombre del paciente/Metadato
        public string? PatientName { get; set; }
        // Edad del paciente DICOM/Metadato
        public string? PatientAge { get; set; }
        // Sexo del paciente DICOM/Metadato
        public string? PatientSex { get; set; }
        // Peso del paciente DICOM/Metadato
        public string? PatientWeight { get; set; }
        // Fecha de nacimiento
        public DateTime? PatientBirthDate { get; set; } // Hacer nullable para manejar casos sin esta información
        public string? IssuerOfPatientID { get; set; } //el emisor del PatientID,
    }
}
