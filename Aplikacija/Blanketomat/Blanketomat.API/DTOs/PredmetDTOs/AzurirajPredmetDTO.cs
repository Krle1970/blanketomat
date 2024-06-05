using Blanketomat.API.Models;

namespace Blanketomat.API.DTOs.PredmetDTOs;

public class AzurirajPredmetDTO : PredmetDTO
{
    public List<Blanket>? Blanketi { get; set; }
}