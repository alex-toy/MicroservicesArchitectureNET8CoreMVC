using System.ComponentModel.DataAnnotations;

namespace Transactions.Web.Dtos.Auth;

public class LoginRequestDto
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}
