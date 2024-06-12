using System.ComponentModel.DataAnnotations;

namespace InterfazBasica_DCStore.Models
{
    public class EstudioConPacienteDto
    {
        // Nombre completo de paciente         
        public string? PatientName { get; set; }
        //ID del paciente
        [StringLength(64)]
        public string? PatientID { get; set; } // ID del paciente DICOM/Metadato
        // Sexo de paciente        
        public string? PatientSex { get; set; }
        // Descripcion sexo
        public string? DescPatientSex { get; set; }
        // Edad de paciente        
        public string? PatientAge { get; set; }
        // Descripcion del estudio DICOM/Metadatos
        public string? StudyDescription { get; set; }
        // Modalidad de estudio Dicom/Metadato        
        public string? Modality { get; set; }
        // Descripcion de modalidad
        public string? DescModality { get; set; }
        // Fecha en que realizó el estudio DICOM/Metadatos        
        public DateTime StudyDate { get; set; }
        // Parte del cuerpo examinada DICOM/Metadatos
        public string? BodyPartExamined { get; set; }
        // Descripcion de parte examinada
        public string? DescBodyPart { get; set; }
        // Nombre de institucion
        public string? InstitutionName { get; set; }
    }
}
