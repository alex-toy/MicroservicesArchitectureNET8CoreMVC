using Auth.Api.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Api.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }

    public UserDto ToDto()
    {
        return new()
        {
            Email = Email,
            Name = Name,
            PhoneNumber = PhoneNumber,
            ID = Id,
        };
    }

    public string GenerateToken(IEnumerable<string> roles, JwtOptions _jwtOptions)
    {
        JwtSecurityTokenHandler tokenHandler = new();

        byte[] key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

        List<Claim> claimList = GetClaims(roles);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Audience = _jwtOptions.Audience,
            Issuer = _jwtOptions.Issuer,
            Subject = new ClaimsIdentity(claimList),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private List<Claim> GetClaims(IEnumerable<string> roles)
    {
        List<Claim> claimList = new()
        {
            new Claim(JwtRegisteredClaimNames.Email, Email),
            new Claim(JwtRegisteredClaimNames.Sub, Id),
            new Claim(JwtRegisteredClaimNames.Name, UserName)
        };

        claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        return claimList;
    }
}
