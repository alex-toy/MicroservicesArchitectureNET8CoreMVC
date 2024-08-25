using Transactions.Web.Dtos;
using Transactions.Web.Dtos.Auth;
using static Transactions.Web.Utils.Constants;

namespace Transactions.Web.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IBaseService _baseService;

    public AuthService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto<LoginResponseDto>?> LoginAsync(LoginRequestDto loginRequestDto)
    {
        return await _baseService.SendAsync(new RequestDto<LoginRequestDto>()
        {
            ApiType = ApiType.POST,
            Data = loginRequestDto,
            Url = AuthAPIBase + "/api/auth/login"
        }, withBearer: false);
    }

    public async Task<ResponseDto<object>?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
    {
        return await _baseService.SendAsync(new RequestDto<RegistrationRequestDto>()
        {
            ApiType = ApiType.POST,
            Data = registrationRequestDto,
            Url = AuthAPIBase + "/api/auth/register"
        }, withBearer: false);
    }

    public async Task<ResponseDto<object>?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
    {
        return await _baseService.SendAsync(new RequestDto<RegistrationRequestDto>()
        {
            ApiType = ApiType.POST,
            Data = registrationRequestDto,
            Url = AuthAPIBase + "/api/auth/AssignRole"
        });
    }
}