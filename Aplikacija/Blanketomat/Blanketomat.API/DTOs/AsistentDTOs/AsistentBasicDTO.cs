using Blanketomat.API.DTOs.KatedraDTOs;
using Blanketomat.API.DTOs.KomentarDTOs;
using Blanketomat.API.DTOs.OdgovorDTOs;
using Blanketomat.API.DTOs.PredmetDTOs;
using Blanketomat.API.DTOs.SmerDTOs;

namespace Blanketomat.API.DTOs.AsistentDTOs;

public class AsistentBasicDTO
{
    public int Id { get; set; }
    public required string Ime { get; set; }
    public required string Prezime { get; set; }
    public required string Email { get; set; }
    public string? Token { get; set; }
    public List<SmerBasicDTO>? Smerovi { get; set; }
    public List<PredmetBasicDTO>? Predmeti { get; set; }
    public KatedraBasicDTO? Katedra { get; set; }
    public List<KomentarBasicDTO>? LajkovaniKomentari { get; set; }
    public List<OdgovorBasicDTO>? LajkovaniOdgovori { get; set; }
}