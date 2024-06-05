using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.DTOs.SmerDTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.SmerFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class SmerController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public SmerController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("smerovi")]
    [TypeFilter(typeof(ValidateDbSetFilter<Smer>))]
    public ActionResult<IEnumerable<Smer>> VratiSveSmerove()
    {
        return Ok(_context.Smerovi);
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Smer>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PagingResponseDTO<Smer>>> VratiSmerove(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Smerovi.Count() / (float)brojRezultata);

        var smerovi = await _context.Smerovi
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PagingResponseDTO<Smer>
        {
            Podaci = smerovi,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Smer>))]
    [TypeFilter(typeof(ValidateIdFilter<Smer>))]
    public ActionResult<Smer> VratiSmer(int id)
    {
        // iz ValidateIdFilter-a
        return Ok(HttpContext.Items["entity"] as Smer);
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDbSetFilter<Smer>))]
    [TypeFilter(typeof(ValidateDodajSmerFilter))]
    public async Task<ActionResult> DodajSmer([FromBody] DodajSmerDTO noviSmer)
    {
        Smer smer = new Smer
        {
            Naziv = noviSmer.Naziv,
            Katedra = noviSmer.Katedra,
            Predmeti = noviSmer.Predmeti,
            Profesori = noviSmer.Profesori,
            Asistenti = noviSmer.Asistenti,
            Studenti = noviSmer.Studenti
        };

        _context.Smerovi.Add(smer);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiSmer),
            new { id = smer.Id },
            smer
            );
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Smer>))]
    [TypeFilter(typeof(ValidateIdFilter<Smer>))]
    public async Task<ActionResult> AzurirajSmer(int id, [FromBody] AzurirajSmerDTO smer)
    {
        // iz ValidateIdFilter-a
        var smerZaAzuriranje = HttpContext.Items["entity"] as Smer;
        smerZaAzuriranje!.Naziv = smer.Naziv;
        smerZaAzuriranje.Katedra = smer.Katedra;
        smerZaAzuriranje.Predmeti = smer.Predmeti;
        smerZaAzuriranje.Profesori = smer.Profesori;
        smerZaAzuriranje.Asistenti = smer.Asistenti;
        smerZaAzuriranje.Studenti = smer.Studenti;

        await _context.SaveChangesAsync();
        return Ok(smerZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Smer>))]
    [TypeFilter(typeof(ValidateIdFilter<Smer>))]
    public async Task<ActionResult<string>> ObrisiSmer(int id)
    {
        // iz ValidateIdFilter-a
        var smerZaBrisanje = HttpContext.Items["entity"] as Smer;
        _context.Smerovi.Remove(smerZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Smer uspešno izbrisan");
    }
}