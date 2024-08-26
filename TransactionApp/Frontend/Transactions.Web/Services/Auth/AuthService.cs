using Transactions.Core.Dtos;
using Transactions.Core.Dtos.Auth;
using Transactions.Core.Services;
using static Transactions.Core.Utils.Constants;

namespace Transactions.Web.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IBaseService _baseService;
    private const string ApiUrl = "/api/auth/";

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
            Url = AuthAPIBase + ApiUrl + "login"
        }, withBearer: false) ?? new ResponseDto<LoginResponseDto> { IsSuccess = false, ErrorMessage = "problem with login" };
    }

    public async Task<ResponseDto<string>> RegisterAsync(RegistrationRequestDto registrationRequestDto)
    {
        return await _baseService.SendAsync<RegistrationRequestDto, string>(new RequestDto<RegistrationRequestDto>()
        {
            ApiType = ApiType.POST,
            Data = registrationRequestDto,
            Url = AuthAPIBase + ApiUrl + "register"
        }, withBearer: false) ?? new ResponseDto<string> { IsSuccess = false, ErrorMessage = "problem with register" };
    }

    public async Task<ResponseDto<string>> AssignRoleAsync(RoleAssignmentDto roleAssignmentDto)
    {
        return await _baseService.SendAsync<RoleAssignmentDto, string>(new RequestDto<RoleAssignmentDto>()
        {
            ApiType = ApiType.POST,
            Data = roleAssignmentDto,
            Url = AuthAPIBase + ApiUrl + "AssignRole"
        }) ?? new ResponseDto<string> { IsSuccess = false, ErrorMessage = "problem with AssignRole" };
    }
}