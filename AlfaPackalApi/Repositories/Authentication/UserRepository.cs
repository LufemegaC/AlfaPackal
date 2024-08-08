using AlfaPackalApi.Datos;
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
using Api_PACsServer.Models.Dto.AuthDtos;
using Api_PACsServer.Repository.DataAccess;
using Api_PACsServer.Repository.IRepository.Authentication;

namespace Api_PACsServer.Repository.Authentication
{
    public class UserRepository : ReadRepository<SystemUser>, IUserRepository
    {

        public UserRepository(ApplicationDbContext db) : base(db)
        {
            //_db = db;
        }

        // 06/08/24 Will be replaced by generic method Get
        //public bool IsUserUnique(string userName)
        //{
        //    var user = _db.SystemUsers.FirstOrDefault(u => u.UserName.ToLower() == userName.ToLower());
        //    return user == null;
        //}

        //    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        //    {
        //        // flag to indicate is a Server
        //        var isServer = (loginRequestDto.LocalIP != null);
        //        // get user from name
        //        var user = await _db.SystemUsers.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
        //        // get institution from user
        //        var InstitutionUser = await _db.Institutions.FirstOrDefaultAsync(u => u.Id == user.InstitutionId);
        //        if (InstitutionUser == null )
        //        {
        //            return new LoginResponseDto()
        //            {
        //                Token = "",
        //                User = null
        //            };
        //        }
        //        // validate password  
        //        bool isValido = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
        //        if (user == null || !isValido)
        //        {
        //            return new LoginResponseDto()
        //            {
        //                Token = "",
        //                User = null
        //            };
        //        }
        //        // Get the role of user
        //        var roles = await _userManager.GetRolesAsync(user);
        //        // tokens handler JWT
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        // convert secret key to bytes
        //        var key = Encoding.ASCII.GetBytes(_secretKey);
        //        // tokens claims
        //        var tokenDescriptor = new SecurityTokenDescriptor
        //        {
        //            Subject = new ClaimsIdentity(new Claim[]
        //            {
        //                new Claim(ClaimTypes.Name, user.UserName),
        //                new Claim(ClaimTypes.Role, roles.FirstOrDefault())       
        //            }),
        //            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        //        };
        //        // configurate life of token
        //        if (isServer)
        //        {
        //            tokenDescriptor.Expires = DateTime.UtcNow.AddDays(1);
        //        }
        //        else
        //        {
        //            tokenDescriptor.Expires = DateTime.UtcNow.AddHours(8);
        //        }
        //        // Create token
        //        var token = tokenHandler.CreateToken(tokenDescriptor);
        //        LoginResponseDto loginResponseDto = new()
        //        {
        //            Token = tokenHandler.WriteToken(token),
        //            User = _mapper.Map<UserDto>(user),
        //            AssignedInstitutionName = InstitutionUser.AssignedInstitutionName,
        //        };
        //        return loginResponseDto;
        //    }

        //    public async Task<UserDto> Register(RegisterRequestDto registerRequestDto)
        //    {
        //        SystemUser user = new()
        //        {
        //            UserName = registerRequestDto.UserName,
        //            FullName = registerRequestDto.FullName,
        //            Email = registerRequestDto.Email,
        //            NormalizedEmail= registerRequestDto.Email.ToUpper(),
        //InstitutionId = registerRequestDto.InstitutionId,
        //            Rol = registerRequestDto.Rol,
        //        };

        //        try
        //        {
        //            var result = await _userManager.CreateAsync(user, registerRequestDto.Password);

        //            if (result.Succeeded)
        //            {
        //                //Valido si el rol existe
        //                if(!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
        //                {
        //                    await _roleManager.CreateAsync(new IdentityRole("admin"));
        //                    await _roleManager.CreateAsync(new IdentityRole("serveradmin"));
        //                    await _roleManager.CreateAsync(new IdentityRole("physician"));
        //                    await _roleManager.CreateAsync(new IdentityRole("technician"));
        //                }
        //                //Asigno rol al usuario
        //                await _userManager.AddToRoleAsync(user, "admin");
        //                var sysUser = _db.SystemUsers.FirstOrDefault(u=>u.UserName== registerRequestDto.UserName);
        //                return _mapper.Map<UserDto>(sysUser);
        //            }
        //        }
        //        catch(Exception ex)
        //        {
        //            throw;
        //        }
        //        return new UserDto();
        //    }
    }
}
