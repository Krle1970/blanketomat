using Blanketomat.API.DTOs.BlanketDTOs;
using Blanketomat.API.DTOs.OdgovorDTOs;

namespace Blanketomat.API.DTOs.KomentarDTOs;

public class KomentarBasicDTO
{
    public required int Id { get; set; }
    public required string Tekst { get; set; }
    public required int Lajkovi { get; set; }
    public BlanketBasicDTO? Blanket { get; set; }
    public List<OdgovorBasicDTO>? Odgovori { get; set; }
    public List<SlikaBasicDTO>? Slike { get; set; }
}