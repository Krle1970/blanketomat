using Blanketomat.API.Models;

namespace Blanketomat.API.DTOs.IspitniRokDTOs;

public class AzurirajIspitniRokDTO : IspitniRokDTO
{
    public List<PonavljanjeIspitnogRoka>? PonavljanjaIspitnihRokova { get; set; }
}