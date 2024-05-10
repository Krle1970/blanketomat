using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.SmerFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SmerController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public SmerController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult> VratiSveSmerove()
    {
        return Ok(await _context.Smerovi.ToListAsync());
    }

    [HttpGet("{page}/{count}")]
    public async Task<ActionResult> VratiSmerove(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Smerovi.Count() / (float)brojRezultata);

        var smerovi = await _context.Smerovi
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Smer>
        {
            Podaci = smerovi,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Smer>))]
    public async Task<ActionResult> VratiSmer(int id)
    {
        return Ok(await _context.Smerovi.FindAsync(id));
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDodajSmerFilter))]
    public async Task<ActionResult> DodajSmer([FromBody] Smer smer)
    {
        _context.Smerovi.Add(smer);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiSmer),
            new { id = smer.Id },
            smer
            );
    }

    [HttpPut]
    [TypeFilter(typeof(ValidateAzurirajSmerFilter))]
    public async Task<ActionResult> AzurirajSmer([FromBody] Smer smer)
    {
        var smerZaAzuriranje = HttpContext.Items["smer"] as Smer;
        smerZaAzuriranje!.Naziv = smer.Naziv;
        smerZaAzuriranje.Predmeti = smer.Predmeti;
        smerZaAzuriranje.Profesori = smer.Profesori;
        smerZaAzuriranje.Asistenti = smer.Asistenti;
        smerZaAzuriranje.Studenti = smer.Studenti;

        await _context.SaveChangesAsync();
        return Ok(smerZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Smer>))]
    public async Task<ActionResult> ObrisiSmer(int id)
    {
        var smerZaBrisanje = await _context.Smerovi.FindAsync(id);
        _context.Smerovi.Remove(smerZaBrisanje!);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}