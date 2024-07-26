namespace InterfazDeUsuario.Models.Identity
{
    public class RegistroRequestDto
    {
        public string UserName { get; set; }
        public string Nombres { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
    }
}
