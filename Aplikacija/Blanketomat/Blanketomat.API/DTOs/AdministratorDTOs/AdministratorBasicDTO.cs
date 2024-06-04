namespace Blanketomat.API.DTOs.AdministratorDTOs;

public class AdministratorBasicDTO
{
    public required int Id { get; set; }
    public required string Ime { get; set; }
    public required string Prezime { get; set; }
    public required string Email { get; set; }
}