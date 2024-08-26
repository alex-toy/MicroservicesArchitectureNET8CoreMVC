using Transactions.Core.Dtos;
using Transactions.Core.Dtos.Auth;

namespace Transactions.Web.Services.Auth;

public interface IAuthService
{
    Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto);
    Task<ResponseDto<string>> RegisterAsync(RegistrationRequestDto registrationRequestDto);
    Task<ResponseDto<string>> AssignRoleAsync(RegistrationRequestDto registrationRequestDto);
}
