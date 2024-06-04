using Blanketomat.API.DTOs.AkreditacijaDTOs;

namespace Blanketomat.API.DTOs.PredmetDTOs;

public class PredmetBasicDTO
{
    public required int Id { get; set; }
    public required string Naziv { get; set; }
    public required string Godina { get; set; }
    public AkreditacijaBasicDTO? Akreditacija { get; set; }
    public List<OblastBasicDTO> MyProperty { get; set; }
}