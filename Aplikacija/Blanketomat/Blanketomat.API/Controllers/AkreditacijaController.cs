using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.DTOs.AkreditacijaDTOs;
using Blanketomat.API.Filters.AkreditacijaFilters;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AkreditacijaController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public AkreditacijaController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("akreditacije")]
    [TypeFilter(typeof(ValidateDbSetFilter<Akreditacija>))]
    public async Task<ActionResult<List<Akreditacija>>> VratiSveAkreditacije()
    {
        List<AkreditacijaBasicDTO> akreditacije = await _context.Akreditacije.
            Select(x => new AkreditacijaBasicDTO
            {
                Id = x.Id,
                Naziv = x.Naziv,
                BrojPredmeta = x.Predmeti == null ? 0 : x.Predmeti.Count(),
                BrojStudenata = x.Studenti == null ? 0 : x.Studenti.Count()
            }).ToListAsync();

        return Ok(akreditacije);
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Akreditacija>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PagingResponseDTO<AkreditacijaPagingResponseDTO>>> VratiAkreditacije(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Akreditacije.Count() / (float)brojRezultata);

        var akreditacija = await _context.Akreditacije
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .Select(x => new AkreditacijaPagingResponseDTO
            {
                Id = x.Id,
                Naziv = x.Naziv,
                BrojPredmeta = x.Predmeti == null ? 0 : x.Predmeti.Count(),
                BrojStudenata = x.Studenti == null ? 0 : x.Studenti.Count()
            }).ToListAsync();

        var response = new PagingResponseDTO<AkreditacijaPagingResponseDTO>
        {
            Podaci = akreditacija,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Akreditacija>))]
    [TypeFilter(typeof(ValidateIdFilter<Akreditacija>))]
    public ActionResult<AkreditacijaBasicDTO> VratiAkreditaciju(int id)
    {
        // iz ValidateIdFilter-a
        Akreditacija? akreditacija = HttpContext.Items["entity"] as Akreditacija;
        AkreditacijaBasicDTO akreditacijaBasic = new AkreditacijaBasicDTO { Id = akreditacija!.Id, Naziv = akreditacija.Naziv, 
            BrojPredmeta = akreditacija.Predmeti == null ? 0 : akreditacija.Predmeti.Count(), 
            BrojStudenata = akreditacija.Studenti == null ? 0 : akreditacija.Studenti.Count()
        };

        return Ok(akreditacijaBasic);
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDbSetFilter<Akreditacija>))]
    [TypeFilter(typeof(ValidateDodajAkreditacijuFilter))]
    public async Task<ActionResult> DodajAkreditaciju([FromBody]DodajAkreditacijuDTO novaAkreditacija)
    {
        Akreditacija akreditacija = new Akreditacija
        {
            Naziv = novaAkreditacija.Naziv,
            Studenti = novaAkreditacija.Studenti,
            Predmeti = novaAkreditacija.Predmeti
        };

        _context.Akreditacije.Add(akreditacija);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiAkreditaciju), 
            new { id = akreditacija.Id },
            akreditacija
            );
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Akreditacija>))]
    [TypeFilter(typeof(ValidateIdFilter<Akreditacija>))]
    public async Task<ActionResult<AkreditacijaBasicDTO>> AzurirajAkreditaciju(int id, [FromBody]AzurirajAkreditacijuDTO akreditacija)
    {
        // iz ValidateIdFilter-a
        var akreditacijaZaAzuriranje = HttpContext.Items["entity"] as Akreditacija;

        akreditacijaZaAzuriranje!.Naziv = akreditacija.Naziv;
        akreditacijaZaAzuriranje.Studenti = akreditacija.Studenti;
        akreditacijaZaAzuriranje.Predmeti = akreditacija.Predmeti;

        AkreditacijaBasicDTO akreditacijaBasic = new AkreditacijaBasicDTO
        {
            Id = akreditacijaZaAzuriranje.Id,
            Naziv = akreditacijaZaAzuriranje.Naziv,
            BrojPredmeta = akreditacijaZaAzuriranje.Predmeti == null ? 0 : akreditacijaZaAzuriranje.Predmeti.Count(),
            BrojStudenata = akreditacijaZaAzuriranje.Studenti == null ? 0 : akreditacijaZaAzuriranje.Studenti.Count()
        };

        await _context.SaveChangesAsync();
        return Ok(akreditacijaBasic);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Akreditacija>))]
    [TypeFilter(typeof(ValidateIdFilter<Akreditacija>))]
    public async Task<ActionResult> ObrisiAkreditaciju(int id)
    {
        // iz ValidateIdFilter-a
        var akreditacijaZaBrisanje = HttpContext.Items["entity"] as Akreditacija;
        _context.Akreditacije.Remove(akreditacijaZaBrisanje!);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}