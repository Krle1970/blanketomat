using Blanketomat.API.DTOs.KatedraDTOs;

namespace Blanketomat.API.DTOs.ProfesorDTO;

public class ProfesorListDTO
{
    public required int Id { get; set; }
    public required string Ime { get; set; }
    public required string Prezime { get; set; }
    public required string Email { get; set; }
    public KatedraIdNazivDTO? Katedra { get; set; }
    public required int BrojLajkovanihKomentara { get; set; }
    public required int BrojLajkovanihOdgovora { get; set; }
}