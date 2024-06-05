using Blanketomat.API.Models;

namespace Blanketomat.API.DTOs.AsistentDTOs;

public class AzurirajAsistentaDTO : AsistentDTO
{
    public List<Komentar>? LajkovaniKomentari { get; set; }
    public List<Odgovor>? LajkovaniOdgovori { get; set; }
}