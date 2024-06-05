using Blanketomat.API.Models;

namespace Blanketomat.API.DTOs.BlanketDTOs;

public class AzurirajBlanketDTO : BlanketDTO
{
    public List<Komentar>? KomentariIds { get; set; }
}