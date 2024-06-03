namespace Blanketomat.API.DTOs.KomentarDTOs;

public class KomentarBasicDTO
{
    public required int Id { get; set; }
    public required string Tekst { get; set; }
    public required int Lajkovi { get; set; }
}