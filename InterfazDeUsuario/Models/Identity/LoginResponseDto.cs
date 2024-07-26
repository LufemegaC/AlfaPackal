namespace InterfazDeUsuario.Models.Identity
{
    public class LoginResponseDto
    {
        public UsuarioDto Usuario { get; set; }
        public string Token { get; set; }
        public string Rol { get; set; }
        public string AssignedInstitutionName  { get; set; }

        public int InstitutionId { get; set; }
    }
}
