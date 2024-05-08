using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PitanjeController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public PitanjeController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("{page}/{count}")]
    public async Task<ActionResult> VratiPitanja(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Pitanja.Count() / (float)brojRezultata);

        var pitanja = await _context.Pitanja
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Pitanje>
        {
            Response = pitanja,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Pitanje>))]
    public async Task<ActionResult> VratiPitanja(int id)
    {
        return Ok(await _context.Oblasti.FindAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult> DodajPitanje([FromBody]Pitanje pitanje)
    {
        await _context.Pitanja.AddAsync(pitanje);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiPitanja),
            new { id = pitanje.Id },
            pitanje);
    }

    [HttpPut]
    public async Task<ActionResult> AzurirajPitanje([FromBody]Pitanje pitanje)
    {
        var pitanjeZaAzuriranje = HttpContext.Items["pitanje"] as Pitanje;

        pitanjeZaAzuriranje!.Tekst=pitanje.Tekst;
        pitanjeZaAzuriranje.Slika=pitanje.Slika;
        pitanjeZaAzuriranje.Oblast=pitanje.Oblast;
        pitanjeZaAzuriranje.Blanketi=pitanje.Blanketi;
        
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Pitanje>))]
    public async Task<ActionResult> ObrisiPitanje(int id)
    {
        var pitanjaZaBrisanje = await _context.Pitanja.FindAsync(id);
        _context.Pitanja.Remove(pitanjaZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok(pitanjaZaBrisanje);
    }
}