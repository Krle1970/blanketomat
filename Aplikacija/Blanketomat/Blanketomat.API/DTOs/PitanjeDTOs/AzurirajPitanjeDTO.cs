using Blanketomat.API.Models;

namespace Blanketomat.API.DTOs.PitanjeDTOs;

public class AzurirajPitanjeDTO : PitanjeDTO
{
    public List<Blanket>? Blanketi { get; set; }
}