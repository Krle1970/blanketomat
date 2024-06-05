using Blanketomat.API.DTOs.AkreditacijaDTOs;
using Blanketomat.API.DTOs.PredmetDTOs;
using Blanketomat.API.DTOs.SmerDTOs;

namespace Blanketomat.API.DTOs.LoginDTOs;

public class StudentLoginResponseDTO
{
    public required string Ime { get; set; }
    public required string Prezime { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Token { get; set; }
    public AkreditacijaIdNazivDTO? Akreditacija { get; set; }
    public SmerIdNazivDTO? Smer { get; set; }
    public List<PredmetIdNazivGodinaDTO>? Predmeti { get; set; }
    public required int BrojKomentara { get; set; }
    public required int BrojOdgovora { get; set; }
}