using Blanketomat.API.Models;

namespace Blanketomat.API.DTOs.PonavljanjeIspitnogRokaDTOs;

public class AzurirajPonavljanjeIspitnogRokaDTO : PonavljanjeIspitnogRokaDTO
{
    public List<Blanket>? Blanketi { get; set; }
}