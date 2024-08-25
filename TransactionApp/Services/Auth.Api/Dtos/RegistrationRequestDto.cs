using Auth.Api.Models;

namespace Auth.Api.Dtos;

public class RegistrationRequestDto
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string? Role { get; set; }

    public ApplicationUser ToApplicationUser()
    {
        return new()
        {
            UserName = Email,
            Email = Email,
            NormalizedEmail = Email.ToUpper(),
            Name = Name,
            PhoneNumber = PhoneNumber
        };
    }
}
