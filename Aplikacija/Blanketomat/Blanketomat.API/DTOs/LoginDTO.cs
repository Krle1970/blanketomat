namespace Blanketomat.API.DTOs;

public class LoginDTO
{
    public string? FullName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string AccountType { get; set;}
}