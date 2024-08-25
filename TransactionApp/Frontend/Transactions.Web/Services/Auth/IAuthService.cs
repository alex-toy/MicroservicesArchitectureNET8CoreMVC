using Transactions.Web.Dtos;
using Transactions.Web.Dtos.Auth;

namespace Transactions.Web.Services.Auth;

public interface IAuthService
{
    Task<ResponseDto<LoginResponseDto>?> LoginAsync(LoginRequestDto loginRequestDto);
    Task<ResponseDto<object>?> RegisterAsync(RegistrationRequestDto registrationRequestDto);
    Task<ResponseDto<object>?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto);
}
