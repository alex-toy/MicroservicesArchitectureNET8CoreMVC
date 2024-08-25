﻿using Auth.Api.Dtos;
using Auth.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthAPIController : ControllerBase
{
    private readonly IAuthService _authService;
    //private readonly IMessageBus _messageBus;
    private readonly IConfiguration _configuration;
    protected ResponseDto _response;

    public AuthAPIController(IAuthService authService,/*IMessageBus messageBus,*/ IConfiguration configuration)
    {
        _authService = authService;
        _configuration = configuration;
        //_messageBus = messageBus;
        _response = new();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
    {
        string errorMessage = await _authService.Register(model);

        if (!string.IsNullOrEmpty(errorMessage))
        {
            _response.IsSuccess = false;
            _response.Message = errorMessage;
            return BadRequest(_response);
        }

        //await _messageBus.PublishMessage(model.Email, _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue"));

        return Ok(_response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
    {
        LoginResponseDto loginResponse = await _authService.Login(model);

        if (loginResponse.User is null)
        {
            _response.IsSuccess = false;
            _response.Message = "Username or password is incorrect";
            return BadRequest(_response);
        }

        _response.Result = loginResponse;
        return Ok(_response);
    }

    [HttpPost("AssignRole")]
    public async Task<IActionResult> AssignRole([FromBody] RoleAssignmentDto model)
    {
        bool assignRoleSuccessful = await _authService.AssignRoles(model);

        if (!assignRoleSuccessful)
        {
            _response.IsSuccess = false;
            _response.Message = "Error encountered";
            return BadRequest(_response);
        }

        return Ok(_response);
    }
}
