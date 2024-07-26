using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos.AccessControl;
using Api_PACsServer.Modelos.Dto;
using Api_PACsServer.Repositorio;
using Api_PACsServer.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api_PACsServer.Controllers.Login
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usuarioRepo;
        private readonly IWhiteListRepositorio _whiteListRepo;
        private APIResponse _response;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioRepositorio usuarioRepo, IWhiteListRepositorio whiteListRepositorio, IMapper mapper)
        {
            _usuarioRepo = usuarioRepo;
            _whiteListRepo = whiteListRepositorio;
            _response = new();
            _mapper = mapper;
        }

        [HttpPost("login")] //api/usuario/login
        public async Task<IActionResult> Login([FromBody] LoginRequestDto modelo)
        {
            try
            {
                // Validacion de IP
                var dicomServer = new DicomServer();
                if (modelo.LocalIP != null)
                {
                    var isValidIp = await _whiteListRepo.IsValidIP(modelo.LocalIP);
                    if (!isValidIp)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsExitoso = false;
                        _response.ErrorsMessages.Add("IP no autorizada");
                        return BadRequest(_response);
                    }
                    //Recupero informacion de servidor
                    dicomServer = await _whiteListRepo.GetServerByIp(modelo.LocalIP);
                }
                //Validacion de usuario
                var loginResponse = await _usuarioRepo.Login(modelo);
                if (loginResponse.Usuario == null || string.IsNullOrEmpty(loginResponse.Token))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    _response.ErrorsMessages.Add("UserName o Password son incorrectos");
                    return BadRequest(_response);
                }
                //Solo si existe server se agrega a respuesta
                if (dicomServer != null)
                {
                    loginResponse.DicomServer = _mapper.Map<DicomServerDto>(dicomServer); ;
                    
                }
                _response.IsExitoso = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Resultado = loginResponse;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                // Preparar la respuesta de error
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsExitoso = false;
                _response.ErrorsMessages.Add("Ocurrió un error inesperado. Por favor, inténtelo nuevamente más tarde.");

                // Devolver la respuesta de error
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            } 
        }

        [HttpPost("registrar")] //api/usuario/login
        public async Task<IActionResult> Registrar([FromBody] RegistroRequestDto modelo)
        {
            bool isUsuarioUnico = _usuarioRepo.IsUsarioUnico(modelo.UserName);
            if (!isUsuarioUnico)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsExitoso = false;
                _response.ErrorsMessages.Add("Usuario ya existe!");
                return BadRequest(_response);
            }
            var usuario = await _usuarioRepo.Registrar(modelo);
            if (usuario == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsExitoso = false;
                _response.ErrorsMessages.Add("Error al registrar usuario!");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsExitoso = true;
            return Ok(_response);

        }
    }
}

