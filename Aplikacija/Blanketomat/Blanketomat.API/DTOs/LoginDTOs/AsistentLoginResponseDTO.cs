using Blanketomat.API.DTOs.KatedraDTOs;
using Blanketomat.API.DTOs.PredmetDTOs;
using Blanketomat.API.DTOs.SmerDTOs;

namespace Blanketomat.API.DTOs.LoginDTOs;

public class AsistentLoginResponseDTO
{
    public required string Ime { get; set; }
    public required string Prezime { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Token { get; set; }
    public List<SmerIdNazivDTO>? Smerovi { get; set; }
    public List<PredmetIdNazivGodinaDTO>? Predmeti { get; set; }
    public KatedraIdNazivDTO? Katedra { get; set; }
    public required int BrojLajkovanihKomentara { get; set; }
    public required int BrojLajkovanihOdgovora { get; set; }
}