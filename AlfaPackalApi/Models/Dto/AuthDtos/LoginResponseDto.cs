using Api_PACsServer.Models.Dto.DicomServer;

namespace Api_PACsServer.Models.Dto.AuthDtos
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public LocalDicomServerDto DicomServer { get; set; }
        public string Token { get; set; }
        //public string AssignedInstitutionName { get; set; }

    }
}
