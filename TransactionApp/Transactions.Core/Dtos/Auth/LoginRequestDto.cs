using System.ComponentModel.DataAnnotations;

namespace Transactions.Core.Dtos.Auth;

public class LoginRequestDto
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}
