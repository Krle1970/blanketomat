namespace Blanketomat.API.DTOs.OblastDTOs;

public class OblastBasicDTO
{
    public required int Id { get; set; }
    public required string Naziv { get; set; }
    public PredmetIdNazivDTO Predmet { get; set; }
}