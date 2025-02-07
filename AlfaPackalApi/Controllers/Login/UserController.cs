using Api_PACsServer.Models.Dto;
using Api_PACsServer.Models.Dto.AuthDtos;
using Api_PACsServer.Orchestrators.IOrchestrator;
using Api_PACsServer.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api_PACsServer.Controllers.Login
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IAuthOrchestrator _authOrchestrator;
        private APIResponse _response;

        public UserController(IAuthOrchestrator authOrchestrator)
        {
            _authOrchestrator = authOrchestrator;
            _response = new();
        }

        [HttpPost("login")] //api/usuario/login
        public async Task<IActionResult> Login([FromBody] LoginRequestDto modelo)
        {
            try
            {
                //Validacion de usuario
                var loginResponse = await _authOrchestrator.Login(modelo);
                if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
                {
                    _response = ConverterHelp.CreateResponse(false, HttpStatusCode.BadRequest, new List<string> { "Username or Password are incorrect" });
                    return BadRequest(_response);
                }
                _response = ConverterHelp.CreateResponse(true, HttpStatusCode.OK, null, loginResponse);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response = ConverterHelp.CreateResponse(false, HttpStatusCode.InternalServerError, new List<string> { ex.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            } 
        }

        [HttpPost("register")] //api/usuario/login
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto modelo)
        {
            try
            {
                var user = await _authOrchestrator.Register(modelo);
                if (user == null)
                {
                    _response = ConverterHelp.CreateResponse(false, HttpStatusCode.BadRequest, new List<string> { "Error registering user" });
                    return BadRequest(_response);
                }
                _response = ConverterHelp.CreateResponse(true, HttpStatusCode.Created, null, user);
                return Ok(_response);
            }

            catch (Exception ex)
            {
                _response = ConverterHelp.CreateResponse(false, HttpStatusCode.InternalServerError, new List<string> { ex.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
    }
}

