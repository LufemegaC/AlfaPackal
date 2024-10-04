using InterfazBasica_DCStore.Service.IService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using InterfazBasica.Models;
using System.IdentityModel.Tokens.Jwt;
using InterfazBasica_DCStore.Models.Dtos.Indentity;
using InterfazBasica_DCStore.Utilities;

namespace InterfazBasica_DCStore.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _usuarioService;
        private readonly string _localIP;

        public UserController(IUserService usuarioService)
        {
            _usuarioService = usuarioService;
            _localIP = LocalUtility.GetLocalIPAddress();
        }
        public async Task<IActionResult> Login()
        {
            var loginRequestDto = new LoginRequestDto(_localIP);
            return View(loginRequestDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDto modelo)
        {
            var response = await _usuarioService.Login<APIResponse>(modelo);
            if (response != null && response.IsSuccessful == true)
            {
                LoginResponseDto loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.ResponseData));
                // config token de acceso
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(loginResponse.Token);
                //Claims
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(c => c.Type == "unique_name").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(c => c.Type == "role").Value));
                // Extraer el valor de InstitutionId
                var institutionIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == "InstitutionId");
                if (institutionIdClaim != null)
                {
                    identity.AddClaim(new Claim("InstitutionId", institutionIdClaim.Value));
                    LocalUtility.InstitutionId = int.Parse(institutionIdClaim.Value);
                }

                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                // Recupero informacion del servidor
                //LocalUtility.Aetitle = loginResponse.DicomServer.AETitle;
                Secret.Token = loginResponse.Token;
                LocalUtility.Aetitle = loginResponse.DicomServer.AETitle;
                TempData["DicomServer"] = JsonConvert.SerializeObject(loginResponse.DicomServer);
                //Token se asigna a la session del usuario
                HttpContext.Session.SetString(Secret.Token, loginResponse.Token);
                //HttpContext.Session.SetString(DS.SessionToken, loginResponse.Token);
                return RedirectToAction("ServerConfig", "Gateway"); 
            }
            else
            {
                ModelState.AddModelError("ErrorMessages", response.ErrorsMessages.FirstOrDefault());
                TempData["error"] = response.ErrorsMessages.FirstOrDefault();
                return View(modelo);

            }
        }
        /// ** REGISTRAR
        public async Task<IActionResult> Registrar()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(Secret.Token, "");
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> AccesoDenegado()
        {
            return View();
        }
    }
}
