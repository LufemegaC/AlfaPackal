using Api_PACsServer.Modelos.AccessControl;

namespace Api_PACsServer.Modelos.Dto
{
    public class LoginResponseDto
    {
        public UsuarioDto Usuario { get; set; }
        public DicomServerDto DicomServer { get; set; }
        public string Token { get; set; }
        public string AssignedInstitutionName { get; set; }

    }
}
