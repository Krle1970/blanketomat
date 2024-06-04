namespace Blanketomat.API.DTOs.AkreditacijaDTOs;

public class AkreditacijaPagingResponseDTO
{
    public required int Id { get; set; }
    public required string Naziv { get; set; }
    public required int BrojPredmeta { get; set; }
    public required int BrojStudenata { get; set; }
}