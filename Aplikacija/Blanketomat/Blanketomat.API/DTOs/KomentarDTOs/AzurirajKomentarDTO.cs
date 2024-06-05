using Blanketomat.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.KomentarDTOs;

public class AzurirajKomentarDTO : KomentarDTO
{
    [Range(0, int.MaxValue, ErrorMessage = "Broj lajkova mora biti pozitivan broj")]
    public int Lajkovi { get; set; }
    public List<Odgovor>? Odgovori { get; set; }
    public List<Profesor>? ProfesoriLiked { get; set; }
    public List<Asistent>? AsistentiLiked { get; set; }
}