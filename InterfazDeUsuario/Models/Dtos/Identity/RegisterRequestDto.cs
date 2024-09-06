namespace InterfazDeUsuario.Models.Dtos.Identity
{
    public class RegisterRequestDto
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
    }
}
