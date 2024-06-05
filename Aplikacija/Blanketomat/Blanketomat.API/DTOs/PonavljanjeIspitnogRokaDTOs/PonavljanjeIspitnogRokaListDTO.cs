using Blanketomat.API.DTOs.IspitniRokDTOs;

namespace Blanketomat.API.DTOs.PonavljanjeIspitnogRokaDTOs;

public class PonavljanjeIspitnogRokaListDTO
{
    public required int Id { get; set; }
    public required string Naziv { get; set; }
    public required DateOnly Datum { get; set; }
    public required IspitniRokIdNazivDTO? IspitniRok { get; set; }
}