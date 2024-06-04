using Blanketomat.API.DTOs.KatedraDTOs;

namespace Blanketomat.API.DTOs.SmerDTOs;

public class SmerListDTO
{
    public required int Id { get; set; }
    public required string Naziv { get; set; }
    public KatedraIdNazivDTO? Katedra { get; set; }
    public required int BrojPredmeta { get; set; }
    public required int BrojProfesora { get; set; }
    public required int BrojAsistenata { get; set; }
    public required int BrojStudenata { get; set; }
}