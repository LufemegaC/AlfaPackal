using Api_PACsServer.Models.Dto.AuthDtos;

namespace Api_PACsServer.Services.IService
{
    public interface ISessionService
    {
        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="loginRequestDto">The login request data transfer object.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the login response data transfer object.</returns>
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="RegisterRequestDto">The registration request data transfer object.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user data transfer object.</returns>
        Task<UserDto> Register(RegisterRequestDto registroRequestDto);

    }
}
