using Blanketomat.API.DTOs.KomentarDTOs;
using Blanketomat.API.DTOs.PredmetDTOs;
using Blanketomat.API.Models;

namespace Blanketomat.API.DTOs.BlanketDTOs;

public class BlanketBasicDTO
{
    public required int Id { get; set; }
    public required string Tip { get; set; }
    public required string Kategorija { get; set; }
    public required string Putanja { get; set; }
    public List<SlikaBasicDTO>? Slike { get; set; }
    public PredmetBasicDTO? Predmet { get; set; }
    public PonavljanjeIspitnogRokaBasicDTO? PonavljanjeIspitnogRoka { get; set; }
    public List<PitanjeBasicDTO>? Pitanja { get; set; }
    public List<ZadatakBasicDTO>? Zadaci { get; set; }
    public List<KomentarBasicDTO>? Komentari { get; set; }
}