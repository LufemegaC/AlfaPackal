using InterfazDeUsuario.Models;
using InterfazDeUsuario.Models.Dtos.Identity;
using InterfazDeUsuario.Services.IServices;
using InterfazDeUsuario.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InterfazDeUsuario.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDto modelo)
        {

            var response = await _userService.Login<APIResponse>(modelo);
            if (response != null && response.IsSuccessful == true)
            {
                LoginResponseDto loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.ResponseData));
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(loginResponse.Token);
                //Claims
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(c => c.Type == "unique_name").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(c => c.Type == "role").Value));
                //
                // Extraer el valor de InstitutionId
                var institutionIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == "InstitutionId");
                if (institutionIdClaim != null)
                {
                    //identity.AddClaim(new Claim("InstitutionId", institutionIdClaim.Value));
                    // Almacenar InstitutionId en la sesión como entero
                    HttpContext.Session.SetInt32(LocalUtility.SessionInstitution, int.Parse(institutionIdClaim.Value));
                }
                //identity.AddClaim(new Claim(ClaimTypes.Role, loginResponse.Usuario.Rol));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                HttpContext.Session.SetString(LocalUtility.SessionToken, loginResponse.Token);
                return RedirectToAction("IndexMainList", "MainListStudies"); 
            }
            else
            {
                ModelState.AddModelError("ErrorMessages", response.ErrorsMessages.FirstOrDefault());
                return View(modelo);
            }
        }
        /// ** REGISTRAR
        public async Task<IActionResult> Registrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(RegisterRequestDto modelo)
        {
            var response = await _userService.Register<APIResponse>(modelo);
            if (response != null && response.IsSuccessful)
            {
                return RedirectToAction("login");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(LocalUtility.SessionToken, "");
            return RedirectToAction("IndexMainList", "MainListStudies");
        }

        public async Task<IActionResult> AccesoDenegado()
        {
            return View();
        }
    }
}
