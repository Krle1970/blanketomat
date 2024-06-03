using Blanketomat.API.DTOs.AkreditacijaDTOs;
using Blanketomat.API.DTOs.KomentarDTOs;
using Blanketomat.API.DTOs.OdgovorDTOs;
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
    public AkreditacijaBasicDTO? Akreditacija { get; set; }
    public SmerBasicDTO? Smer { get; set; }
    public List<PredmetBasicDTO>? Predmeti { get; set; }
    public List<KomentarBasicDTO>? Komentari { get; set; }
    public List<OdgovorBasicDTO>? Odgovori { get; set; }
}