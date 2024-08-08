namespace Api_PACsServer.Models.Dto.AuthDtos
{
    public class LoginRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? LocalIP { get; set; }
    }
}
