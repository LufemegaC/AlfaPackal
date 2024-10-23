using Api_PACsServer.Modelos.AccessControl;
using Api_PACsServer.Models.Dto.AuthDtos;
using Api_PACsServer.Models.Dto.DicomServer;
using Api_PACsServer.Repository.IRepository.Authentication;
using Api_PACsServer.Services.IService;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Api_PACsServer.Services
{
    public class SessionService : ISessionService
    {
        // Repositories
        private readonly IUserRepository _userRepository;
        private readonly ILocalDicomServerRepostory _localDicomServerRepository;
        //private readonly IInstitutionRespository _institutionRepository;

        // Mapping
        private readonly IMapper _mapper;

        // Dto reponse
        private LoginResponseDto _LoginResponse;
        
        // 
        private string _secretKey;
        // managers of indentity
        private readonly UserManager<SystemUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        

        public SessionService(IUserRepository userRepository, IConfiguration configuration, ILocalDicomServerRepostory localDicomServerRepository, 
                              IMapper mapper,UserManager<SystemUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _userRepository = userRepository;
            _localDicomServerRepository = localDicomServerRepository;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
        {
            // Validacion de IP
            var localIP = loginRequest.LocalIP;
            var isServer = !string.IsNullOrEmpty(localIP);
            LocalDicomServer localServer = null;
            if (isServer)
            {
                localServer = await _localDicomServerRepository.Get(u => u.IP == localIP);
                if (localServer == null)
                {
                    throw new ArgumentException("Server not found.");
                }

                if (!localServer.IsActive)
                {
                    throw new ArgumentException("Server found but not active.");
                }
            }
            //Busca usuario y contraseña en la DBs
            var user = await _userRepository.Get(u => u.UserName.ToLower() == loginRequest.UserName.ToLower());
            if(user == null)
            {
                throw new ArgumentException("User not found.");
            }
            // Si no se encontró  
            bool isValido = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (user == null || !isValido)
            {
                throw new ArgumentException("Incorrect password.");
            }
            //
            var roles = await _userManager.GetRolesAsync(user);
            //manejador de tokens JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            // Convierte la clave secreta en un arreglo de bytes
            var key = Encoding.ASCII.GetBytes(_secretKey);
            // Define las propiedades del token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                    //new Claim("InstitutionId", user.InstitutionId.ToString()) // Agrega el Id de la institución del usuario como un 'Claim' personalizado
                }),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            LocalDicomServerDto serverDto = null;
            if (isServer && localServer != null)
            {
                tokenDescriptor.Expires = DateTime.UtcNow.AddHours(8);
                serverDto = _mapper.Map<LocalDicomServerDto>(localServer);
            }
            else
            {
                tokenDescriptor.Expires = DateTime.UtcNow.AddDays(1);
            }
            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDto loginResponseDto = new()
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<UserDto>(user),
            };
            if(isServer && serverDto != null)
            {
                loginResponseDto.DicomServer = serverDto;
            }
            return loginResponseDto;
        }

        public async Task<UserDto> Register(RegisterRequestDto registerRequest)
        {
            if (await _userRepository.Exists(U => U.UserName == registerRequest.UserName))
            {
                throw new ArgumentException("User already exists!");
            }
            // Utiliza AutoMapper para mapear RegisterRequestDto a SystemUser
            SystemUser user = _mapper.Map<SystemUser>(registerRequest);
            try
            {
                var resultCreate = await _userManager.CreateAsync(user, registerRequest.Password);
                if (resultCreate.Succeeded)
                {
                    //Valido si el rol existe
                    if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Admin"));
                        await _roleManager.CreateAsync(new IdentityRole("serverAdmin"));
                        await _roleManager.CreateAsync(new IdentityRole("Physician"));
                        await _roleManager.CreateAsync(new IdentityRole("Technician"));
                    }
                    //Asigno rol al usuario
                    await _userManager.AddToRoleAsync(user, "Admin");
                    var userSys = _userRepository.Get(u => u.UserName == registerRequest.UserName);
                    return _mapper.Map<UserDto>(userSys);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            return new UserDto();
        }
    }
}
