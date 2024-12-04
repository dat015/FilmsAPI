using FilmsAPI.DTO;

namespace FilmsAPI.Services.AuthService
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> LoginAsync(LoginDTO model);
        Task<LoginResponseDTO> RegisterAsync(RegisterDTO model);
        string GenerateJwtToken(string username, string role);
    }
}
