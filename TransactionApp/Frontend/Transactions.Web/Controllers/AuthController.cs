using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Transactions.Core.Dtos;
using Transactions.Core.Dtos.Auth;
using Transactions.Core.Utils;
using Transactions.Web.Services.Auth;

namespace Transactions.Web.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ITokenProvider _tokenProvider;

    public AuthController(IAuthService authService, ITokenProvider tokenProvider)
    {
        _authService = authService;
        _tokenProvider = tokenProvider;
    }

    [HttpGet]
    public IActionResult Login()
    {
        LoginRequestDto loginRequestDto = new();
        return View(loginRequestDto);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestDto loginRequest)
    {
        ResponseDto<LoginResponseDto> responseDto = await _authService.LoginAsync(loginRequest);

        if (!responseDto.IsSuccess)
        {
            TempData["error"] = responseDto.ErrorMessage;
            return View(loginRequest);
        }

        LoginResponseDto loginResponseDto = responseDto.Result;

        await SignInUser(loginResponseDto);
        _tokenProvider.SetToken(loginResponseDto.Token);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        ViewBag.RoleList = PopulateRoleDropdown();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegistrationRequestDto registrationRequest)
    {
        ResponseDto<string> result = await _authService.RegisterAsync(registrationRequest);
        ResponseDto<string>? assingRole;

        if (result.IsSuccess)
        {
            if (string.IsNullOrEmpty(registrationRequest.Role)) registrationRequest.Role = Constants.RoleCustomer;

            assingRole = await _authService.AssignRoleAsync(registrationRequest);

            if (assingRole.IsSuccess)
            {
                TempData["success"] = "Registration Successful";
                return RedirectToAction(nameof(Login));
            }
        }
        else
        {
            TempData["error"] = result.ErrorMessage;
        }

        ViewBag.RoleList = PopulateRoleDropdown();

        return View(registrationRequest);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        _tokenProvider.ClearToken();
        return RedirectToAction("Index", "Home");
    }


    private async Task SignInUser(LoginResponseDto model)
    {
        JwtSecurityTokenHandler handler = new();

        JwtSecurityToken jwt = handler.ReadJwtToken(model.Token);

        ClaimsIdentity identity = GetClaimsIdentity(jwt);

        ClaimsPrincipal principal = new(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }

    private static ClaimsIdentity GetClaimsIdentity(JwtSecurityToken jwt)
    {
        ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
        identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
        identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
        return identity;
    }

    private static List<SelectListItem> PopulateRoleDropdown()
    {
        return new()
        {
            new SelectListItem{ Text = Constants.RoleAdmin, Value = Constants.RoleAdmin},
            new SelectListItem{ Text = Constants.RoleCustomer, Value = Constants.RoleCustomer},
        };
    }
}