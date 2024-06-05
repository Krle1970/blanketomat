using Blanketomat.API.DTOs.SmerDTOs;

namespace Blanketomat.API.DTOs.KatedraDTOs;

public class KatedraBasicDTO
{
    public required int Id { get; set; }
    public required string Naziv { get; set; }
    public List<SmerBasicDTO>? Smerovi { get; set; }
    public required int BrojProfesora { get; set; }
    public required int BrojAsistenata { get; set; }
}