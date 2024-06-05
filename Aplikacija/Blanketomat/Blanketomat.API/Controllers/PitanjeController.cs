using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.DTOs.PitanjeDTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.PitanjeFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PitanjeController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public PitanjeController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Pitanje>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PagingResponseDTO<Pitanje>>> VratiPitanja(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Pitanja.Count() / (float)brojRezultata);

        var pitanja = await _context.Pitanja
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PagingResponseDTO<Pitanje>
        {
            Podaci = pitanja,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Pitanje>))]
    [TypeFilter(typeof(ValidateIdFilter<Pitanje>))]
    public ActionResult<Pitanje> VratiPitanje(int id)
    {
        // iz ValidateIdFilter-a
        return Ok(HttpContext.Items["entity"] as Pitanje);
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDbSetFilter<Pitanje>))]
    [TypeFilter(typeof(ValidateDodajPitanjeFilter))]
    public async Task<ActionResult> DodajPitanje([FromBody]DodajPitanjeDTO novoPitanje)
    {
        Pitanje pitanje = new Pitanje
        {
            Tekst = novoPitanje.Tekst,
            Slika = novoPitanje.Slike,
            Oblast = novoPitanje.Oblast
        };

        _context.Pitanja.Add(pitanje);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiPitanje),
            new { id = pitanje.Id },
            pitanje
            );
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Pitanje>))]
    public async Task<ActionResult<Pitanje>> AzurirajPitanje(int id, [FromBody]AzurirajPitanjeDTO pitanje)
    {
        // iz ValidateIdFilter-a
        var pitanjeZaAzuriranje = HttpContext.Items["entity"] as Pitanje;

        pitanjeZaAzuriranje!.Tekst = pitanje.Tekst;
        pitanjeZaAzuriranje.Slika = pitanje.Slike;
        pitanjeZaAzuriranje.Oblast = pitanje.Oblast;
        pitanjeZaAzuriranje.Blanketi = pitanje.Blanketi;

        await _context.SaveChangesAsync();
        return Ok(pitanjeZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Pitanje>))]
    [TypeFilter(typeof(ValidateIdFilter<Pitanje>))]
    public async Task<ActionResult<string>> ObrisiPitanje(int id)
    {
        // iz ValidateIdFilter-a
        var pitanjeZaBrisanje = HttpContext.Items["entity"] as Pitanje;
        _context.Pitanja.Remove(pitanjeZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Pitanje uspešno obrisano");
    }
}