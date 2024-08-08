using Api_PACsServer.Models.Dto.AuthDtos;
using Api_PACsServer.Orchestrators.IOrchestrator;
using Api_PACsServer.Services.IService;
using Api_PACsServer.Services.IService.Pacs;

namespace Api_PACsServer.Orchestrators
{
    public class AuthOrchestrator : IAuthOrchestrator
    {

        private readonly ISessionService _sessionService;

        public AuthOrchestrator(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
        {
            return await _sessionService.Login(loginRequest);
        }

        public async Task<UserDto> Register(RegisterRequestDto registerRequest)
        {
            return await _sessionService.Register(registerRequest);
        }
    }
}
