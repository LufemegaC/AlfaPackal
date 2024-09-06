namespace InterfazBasica_DCStore.Models.Dtos.Indentity
{
    public class LoginRequestDto
    {
        public LoginRequestDto() { }
        public LoginRequestDto(string localIp) 
        {
            LocalIP = localIp;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string LocalIP { get; set; }
    }
}
