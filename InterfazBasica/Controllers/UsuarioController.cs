using InterfazBasica_DCStore.Service.IService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using Utileria;
using InterfazBasica_DCStore.Models.Indentity;
using InterfazBasica.Models;
using System.IdentityModel.Tokens.Jwt;
using InterfazBasica_DCStore.Models.Pacs;
using Newtonsoft.Json.Linq;

namespace InterfazBasica_DCStore.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly string _localIP;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
            _localIP = GeneralFunctions.GetLocalIPAddress();
        }
        public async Task<IActionResult> Login()
        {
            var loginRequestDto = new LoginRequestDto
            {
                LocalIP = _localIP,
            };
            return View(loginRequestDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDto modelo)
        {
            var response = await _usuarioService.Login<APIResponse>(modelo);
            if (response != null && response.IsExitoso == true)
            {
                LoginResponseDto loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Resultado));
                // config token de acceso
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(loginResponse.Token);
                //Claims
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(c => c.Type == "unique_name").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(c => c.Type == "role").Value));
                //identity.AddClaim(new Claim(ClaimTypes.Role, loginResponse.Usuario.Rol));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                // Recupero informacion del servidor
                ServerInfo.Aetitle = loginResponse.DicomServer.AETitle;
                ServerInfo.Token = loginResponse.Token;
                ServerInfo.InstitutionId = loginResponse.DicomServer.InstitutionId;
                TempData["DicomServer"] = JsonConvert.SerializeObject(loginResponse.DicomServer);
                //Token se asigna a la session del usuario
                HttpContext.Session.SetString(ServerInfo.SessionToken, loginResponse.Token);
                //HttpContext.Session.SetString(DS.SessionToken, loginResponse.Token);
                return RedirectToAction("MenuPrincipal", "Gateway"); 
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(RegistroRequestDto modelo)
        {
            var response = await _usuarioService.Registar<APIResponse>(modelo);
            if (response != null && response.IsExitoso)
            {
                return RedirectToAction("login");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(DS.SessionToken, "");
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> AccesoDenegado()
        {
            return View();
        }
    }
}
