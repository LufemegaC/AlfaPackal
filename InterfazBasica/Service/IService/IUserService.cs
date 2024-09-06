using InterfazBasica_DCStore.Models.Dtos.Indentity;

namespace InterfazBasica_DCStore.Service.IService
{
    public interface IUserService
    {
        /// <summary>
        /// Authenticates a user based on the provided login request data.
        /// </summary>
        /// <typeparam name="T">The type of the expected response.</typeparam>
        /// <param name="dto">The data transfer object containing the user's login credentials.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the response of type <typeparamref name="T"/>.</returns>
        Task<T> Login<T>(LoginRequestDto loginRequest);
    }
}
