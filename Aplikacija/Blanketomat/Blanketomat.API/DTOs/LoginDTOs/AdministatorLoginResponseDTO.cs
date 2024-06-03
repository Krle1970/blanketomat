namespace Blanketomat.API.DTOs.LoginDTOs;

public class AdministatorLoginResponseDTO
{
    public required string Ime { get; set; }
    public required string Prezime { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Token { get; set; }
}