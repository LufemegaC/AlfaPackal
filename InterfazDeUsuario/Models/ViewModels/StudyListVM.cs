using InterfazDeUsuario.Models.Pacs;

namespace InterfazDeUsuario.Models.ViewModels
{
    public class StudyListVM
    {
        public string DescModality { get; set; }
        public string DescBodyPart { get; set; }
        public string DescPatientSex { get; set; }
        public EstudioDto Estudio { get; set; }
        public PacienteDto Paciente { get; set; }
    }
}
