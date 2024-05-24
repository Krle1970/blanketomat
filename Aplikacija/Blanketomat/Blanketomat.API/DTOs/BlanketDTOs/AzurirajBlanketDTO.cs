using Blanketomat.API.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.BlanketDTOs;

public class AzurirajBlanketDTO : BlanketDTO
{
    public int[]? KomentariIds { get; set; }
}