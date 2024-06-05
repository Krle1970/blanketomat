using Blanketomat.API.DTOs.AkreditacijaDTOs;
using Blanketomat.API.DTOs.AsistentDTOs;
using Blanketomat.API.DTOs.KatedraDTOs;
using Blanketomat.API.DTOs.OblastDTOs;
using Blanketomat.API.DTOs.PodoblastDTOs;
using Blanketomat.API.DTOs.ProfesorDTO;
using Blanketomat.API.DTOs.SmerDTOs;

namespace Blanketomat.API.DTOs.PredmetDTOs;

public class PredmetListDTO
{
    public required int Id { get; set; }
    public required string Naziv { get; set; }
    public required string Godina { get; set; }
    public AkreditacijaIdNazivDTO? Akreditacija { get; set; }
    public List<OblastIdNazivDTO>? Oblasti { get; set; }
    public List<PodoblastIdNazivDTO>? Podoblasti { get; set; }
    public SmerIdNazivDTO? Smer { get; set; }
    public KatedraIdNazivDTO? Katedra { get; set; }
    public List<ProfesorIdImePrezimeDTO>? Profesori { get; set; }
    public List<AsistentIdImePrezimeDTO>? Asistenti { get; set; }
    public required int BrojStudenata { get; set; }
}