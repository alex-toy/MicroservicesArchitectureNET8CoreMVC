namespace Auth.Api.Dtos;

public class RoleAssignmentDto
{
    public  string Email { get; set; }
    public List<string> Roles { get; set; }
}
