using InterfazBasica_DCStore.Models.Dtos.Server;

namespace InterfazBasica_DCStore.Models.Dtos.Indentity
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public DicomServerDto DicomServer { get; set; }
        public string Token { get; set; }
    }
}
