using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.IspitniRokDTOs;

public class AzurirajIspitniRokDTO : IspitniRokDTO
{
    public int[]? PonavljanjaIspitnihRokovaIds { get; set; }
}