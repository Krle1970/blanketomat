using Blanketomat.API.Models;

namespace Blanketomat.API.DTOs.PodoblastDTOs;

public class AzurirajPodoblastDTO : PodoblastDTO
{
    public List<Blanket>? Blanketi { get; set; }
}