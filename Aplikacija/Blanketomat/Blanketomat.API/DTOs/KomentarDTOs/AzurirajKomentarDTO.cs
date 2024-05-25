using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.KomentarDTOs;

public class AzurirajKomentarDTO : KomentarDTO
{
    [Range(0, int.MaxValue, ErrorMessage = "Broj lajkova mora biti pozitivan broj")]
    public int Lajkovi { get; set; }
    public int[]? OdgovoriIds { get; set; }
    public int[]? ProfesoriLikedIds { get; set; }
    public int[]? AsistentiLikedIds { get; set; }
}