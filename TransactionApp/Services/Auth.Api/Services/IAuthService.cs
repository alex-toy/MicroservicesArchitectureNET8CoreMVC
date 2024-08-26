using Transactions.Core.Dtos.Auth;

namespace Auth.Api.Services
{
    public interface IAuthService
    {
        Task<bool> AssignRoles(RoleAssignmentDto roleAssignmentDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
    }
}