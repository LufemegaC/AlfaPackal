 using AlfaPackalApi.Modelos.Dto.Pacs;

namespace Api_PACsServer.Modelos.Dto.Vistas
{
    public class StudyListDto
    {
        public string DescModality { get; set; }
        public string DescBodyPart { get; set; }
        public string DescPatientSex { get; set; }

        public EstudioDto Estudio { get; set; }
        public PacienteDto Paciente { get; set; }
    }
}
