using Auth.Api.Dtos;

namespace Auth.Api.Services
{
    public interface IAuthService
    {
        Task<bool> AssignRole(string email, string roleName);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
    }
}