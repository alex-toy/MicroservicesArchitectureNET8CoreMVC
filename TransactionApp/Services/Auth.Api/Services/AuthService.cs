using Auth.Api.Data;
using Auth.Api.Dtos;
using Auth.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Auth.Api.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JwtOptions _jwtOptions;

    public AuthService(AppDbContext db, IOptions<JwtOptions> jwtOptions, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _db = db;
        _jwtOptions = jwtOptions.Value;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        ApplicationUser? user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

        if (user is null) return new LoginResponseDto() { User = null, Token = "" };

        bool isPasswordValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

        if (!isPasswordValid) return new LoginResponseDto() { User = null, Token = "" };

        IList<string> roles = await _userManager.GetRolesAsync(user);

        string token = user.GenerateToken(roles, _jwtOptions);

        return new ()
        {
            User = user.ToDto(),
            Token = token
        };
    }

    public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
    {
        ApplicationUser user = registrationRequestDto.ToApplicationUser();

        try
        {
            IdentityResult result = await _userManager.CreateAsync(user, registrationRequestDto.Password);

            if (!result.Succeeded) return result.Errors.FirstOrDefault().Description;

            ApplicationUser userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);

            UserDto userDto = userToReturn.ToDto();

            return "";
        }
        catch (Exception ex)
        {

        }

        return "Error Encountered";
    }

    public async Task<bool> AssignRole(string email, string roleName)
    {
        ApplicationUser? user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

        if (user is null) return false;

        if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        await _userManager.AddToRoleAsync(user, roleName);

        return true;
    }
}
