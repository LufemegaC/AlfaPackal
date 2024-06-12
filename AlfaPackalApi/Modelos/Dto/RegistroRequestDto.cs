namespace Api_PACsServer.Modelos.Dto
{
    public class RegistroRequestDto
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
		public int InstitutionId { get; set; }
    }
}
