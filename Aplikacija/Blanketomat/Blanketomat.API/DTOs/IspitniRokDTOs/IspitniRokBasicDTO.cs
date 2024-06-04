namespace Blanketomat.API.DTOs.IspitniRokDTOs;

public class IspitniRokBasicDTO
{
    public required int Id { get; set; }
    public required string Naziv { get; set; }
    public List<PonavljanjeIspitnogRokaBasicDTO>? Ponavlanja { get; set; }
}