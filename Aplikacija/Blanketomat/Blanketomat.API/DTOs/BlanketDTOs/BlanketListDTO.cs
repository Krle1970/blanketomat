using Blanketomat.API.DTOs.AkreditacijaDTOs;
using Blanketomat.API.DTOs.OblastDTOs;
using Blanketomat.API.DTOs.PodoblastDTOs;
using Blanketomat.API.DTOs.PonavljanjeIspitnogRokaDTOs;
using Blanketomat.API.DTOs.PredmetDTOs;

namespace Blanketomat.API.DTOs.BlanketDTOs;

public class BlanketListDTO
{
    public required int Id { get; set; }
    public required string Tip { get; set; }
    public required string Kategorija { get; set; }
    public PredmetIdNazivGodinaDTO? Predmet { get; set; }
    public AkreditacijaIdNazivDTO? Akreditacija { get; set; }
    public PonavljanjeIspitnogRokaIdNazivDTO? IspitniRok { get; set; }
    public List<OblastIdNazivDTO>? Oblasti { get; set; }
    public List<PodoblastIdNazivDTO>? Podoblasti { get; set; }
    public required int BrojPitanja { get; set; }
    public required int BrojZadataka { get; set; }
    public required int BrojKomentara { get; set; }
    public required int BrojOdgovora { get; set; }
}