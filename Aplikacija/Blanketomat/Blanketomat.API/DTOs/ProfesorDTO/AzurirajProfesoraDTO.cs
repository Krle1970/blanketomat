using Blanketomat.API.Models;

namespace Blanketomat.API.DTOs.ProfesorDTO;

public class AzurirajProfesoraDTO : ProfesorDTO
{
    public List<Komentar>? LajkovaniKomentari { get; set; }
    public List<Odgovor>? LajkovaniOdgovori { get; set; }
}