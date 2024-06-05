using Blanketomat.API.DTOs.SmerDTOs;

namespace Blanketomat.API.DTOs.KatedraDTOs;

public class KatedraListDTO
{
    public required int Id { get; set; }
    public required string Naziv { get; set; }
    public List<SmerIdNazivDTO>? Smerovi { get; set; }
    public required int BrojProfesora { get; set; }
    public required int BrojAsistenata { get; set; }
}