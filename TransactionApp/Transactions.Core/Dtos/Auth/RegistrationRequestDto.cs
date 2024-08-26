using Auth.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Transactions.Core.Dtos.Auth;

public class RegistrationRequestDto
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
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
