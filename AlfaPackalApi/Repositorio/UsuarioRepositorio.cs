using AlfaPackalApi.Datos;
using Api_PACsServer.Modelos.Dto;
using Api_PACsServer.Modelos;
using Api_PACsServer.Repositorio.IRepositorio;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Api_PACsServer.Modelos.AccessControl;
using AutoMapper;

namespace Api_PACsServer.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _db;

        private string _secretKey;
        private readonly UserManager<UsuarioSistema> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UsuarioRepositorio(ApplicationDbContext db, IConfiguration configuration, UserManager<UsuarioSistema> userManager,
            IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public bool IsUsarioUnico(string userName)
        {
            var usuario = _db.UsuariosSistema.FirstOrDefault(u => u.UserName.ToLower() == userName.ToLower());
            return usuario == null;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {

            //Busca usuario y contraseña en la DBs
            var usuario = await _db.UsuariosSistema.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            //Institucion del usuario
            var InstitutionUser = await _db.Institutions.FirstOrDefaultAsync(u => u.Id == usuario.InstitutionId);
            if (InstitutionUser == null )
            {
                return new LoginResponseDto()
                {
                    Token = "",
                    Usuario = null
                };
            }
            // Si no se encontró  
            bool isValido = await _userManager.CheckPasswordAsync(usuario, loginRequestDto.Password);
            if (usuario == null || !isValido)
            {
                return new LoginResponseDto()
                {
                    Token = "",
                    Usuario = null
                };
            }
            var roles = await _userManager.GetRolesAsync(usuario);
            //manejador de tokens JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            // Convierte la clave secreta en un arreglo de bytes
            var key = Encoding.ASCII.GetBytes(_secretKey);

            // Define las propiedades del token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                    //new Claim("InstitutionId", usuario.InstitutionId.ToString()) // Agrega el Id de la institución del usuario como un reclamo personalizado
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDto loginResponseDto = new()
            {
                Token = tokenHandler.WriteToken(token),
                Usuario = _mapper.Map<UsuarioDto>(usuario),
                AssignedInstitutionName = InstitutionUser.AssignedInstitutionName,
            };
            return loginResponseDto;

        }

        public async Task<UsuarioDto> Registrar(RegistroRequestDto registroRequestDto)
        {
            UsuarioSistema usuario = new()
            {
                UserName = registroRequestDto.UserName,
                FullName = registroRequestDto.FullName,
                Email = registroRequestDto.Email,
                NormalizedEmail= registroRequestDto.Email.ToUpper(),
				InstitutionId = registroRequestDto.InstitutionId,
                Rol = registroRequestDto.Rol,
                //Rol = registroRequestDto.Rol,
            };

            try
            {
                var resultado = await _userManager.CreateAsync(usuario, registroRequestDto.Password);

                if (resultado.Succeeded)
                {
                    //Valido si el rol existe
                    if(!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("serveradmin"));
                        await _roleManager.CreateAsync(new IdentityRole("physician"));
                    }
                    //Asigno rol al usuario
                    await _userManager.AddToRoleAsync(usuario, "admin");
                    var usuarioSis = _db.UsuariosSistema.FirstOrDefault(u=>u.UserName== registroRequestDto.UserName);
                    return _mapper.Map<UsuarioDto>(usuarioSis);
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            return new UsuarioDto();
        }
    }
}
