using Transactions.Core.Dtos;
using Transactions.Core.Dtos.Auth;
using Transactions.Core.Services;
using static Transactions.Core.Utils.Constants;

namespace Transactions.Web.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IBaseService _baseService;

    public AuthService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto)
    {
        return await _baseService.SendAsync<LoginRequestDto, LoginResponseDto>(new RequestDto<LoginRequestDto>()
        {
            ApiType = ApiType.POST,
            Data = loginRequestDto,
            Url = AuthAPIBase + "/api/auth/login"
        }, withBearer: false) ?? new ResponseDto<LoginResponseDto> { IsSuccess = false, ErrorMessage = "problem with login" };
    }

    public async Task<ResponseDto<string>> RegisterAsync(RegistrationRequestDto registrationRequestDto)
    {
        return await _baseService.SendAsync<RegistrationRequestDto, string>(new RequestDto<RegistrationRequestDto>()
        {
            ApiType = ApiType.POST,
            Data = registrationRequestDto,
            Url = AuthAPIBase + "/api/auth/register"
        }, withBearer: false) ?? new ResponseDto<string> { IsSuccess = false, ErrorMessage = "problem with register" };
    }

    public async Task<ResponseDto<string>> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
    {
        return await _baseService.SendAsync<RegistrationRequestDto, string>(new RequestDto<RegistrationRequestDto>()
        {
            ApiType = ApiType.POST,
            Data = registrationRequestDto,
            Url = AuthAPIBase + "/api/auth/AssignRole"
        }) ?? new ResponseDto<string> { IsSuccess = false, ErrorMessage = "problem with AssignRole" };
    }
}