using Auth.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Transactions.Core.Dtos;
using Transactions.Core.Dtos.Auth;

namespace Auth.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthAPIController : ControllerBase
{
    private readonly IAuthService _authService;
    //private readonly IMessageBus _messageBus;
    private readonly IConfiguration _configuration;

    public AuthAPIController(IAuthService authService,/*IMessageBus messageBus,*/ IConfiguration configuration)
    {
        _authService = authService;
        _configuration = configuration;
        //_messageBus = messageBus;
    }

    [HttpPost("login")]
    public async Task<ResponseDto<LoginResponseDto>> Login([FromBody] LoginRequestDto model)
    {
        LoginResponseDto loginResponse = await _authService.Login(model);

        if (loginResponse.User is null)
        {
            return new ResponseDto<LoginResponseDto> { ErrorMessage = "Username or password is incorrect", IsSuccess = false };
        }

        return new ResponseDto<LoginResponseDto> { Result = loginResponse, IsSuccess = true };
    }

    [HttpPost("register")]
    public async Task<ResponseDto<string>> Register([FromBody] RegistrationRequestDto model)
    {
        string errorMessage = await _authService.Register(model);

        if (!string.IsNullOrEmpty(errorMessage))
        {
            return new ResponseDto<string> { ErrorMessage = errorMessage, IsSuccess = false };
        }

        //await _messageBus.PublishMessage(model.Email, _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue"));

        return new ResponseDto<string> { IsSuccess = true };
    }

    [HttpPost("AssignRole")]
    public async Task<ResponseDto<string>> AssignRole([FromBody] RoleAssignmentDto model)
    {
        bool assignRoleSuccessful = await _authService.AssignRoles(model);

        if (!assignRoleSuccessful)
        {
            return new ResponseDto<string> { ErrorMessage = "Error encountered", IsSuccess = false };
        }

        return new ResponseDto<string> { IsSuccess = true };
    }
}
