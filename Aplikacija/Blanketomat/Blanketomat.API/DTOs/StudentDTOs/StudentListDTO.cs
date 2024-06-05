using Blanketomat.API.DTOs.AkreditacijaDTOs;
using Blanketomat.API.DTOs.SmerDTOs;

namespace Blanketomat.API.DTOs.StudentDTOs;

public class StudentListDTO
{
    public required int Id { get; set; }
    public required string Ime { get; set; }
    public required string Prezime { get; set; }
    public required string Email { get; set; }
    public AkreditacijaIdNazivDTO? Akreditacija { get; set; }
    public SmerIdNazivDTO? Smer { get; set; }
    public required int BrojPostavljenihKomentara { get; set; }
    public required int BrojPostavljenihOdgovora { get; set; }
}