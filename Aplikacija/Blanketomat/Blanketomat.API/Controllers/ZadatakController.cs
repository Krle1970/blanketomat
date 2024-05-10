using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ZadatakController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public ZadatakController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("{page}/{count}")]
    public async Task<ActionResult> VratiZadatke(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Zadaci.Count() / (float)brojRezultata);

        var zadaci = await _context.Zadaci
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Zadatak>
        {
            Podaci = zadaci,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Zadatak>))]
    public async Task<ActionResult> VratiZadatak(int id)
    {
        return Ok(await _context.Zadaci.FindAsync(id));
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDodajZadatakFilter))]
    public async Task<ActionResult> DodajZadatak([FromBody] Zadatak zadatak)
    {
        _context.Zadaci.Add(zadatak);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiZadatak),
            new { id = zadatak.Id },
            zadatak
            );
    }

    [HttpPut]
    [TypeFilter(typeof(ValidateAzurirajZadatakFilter))]
    public async Task<ActionResult> AzurirajZadatak([FromBody] Zadatak zadatak)
    {
        var zadatakZaAzuriranje = HttpContext.Items["zadatak"] as Zadatak;
        zadatakZaAzuriranje!.Tekst = zadatak.Tekst;
        zadatakZaAzuriranje.Slika = zadatak.Slika;
        zadatakZaAzuriranje.Oblast = zadatak.Oblast;
        zadatakZaAzuriranje.Podoblast = zadatak.Podoblast;
        zadatakZaAzuriranje.Blanketi = zadatak.Blanketi;

        await _context.SaveChangesAsync();
        return Ok(zadatakZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Zadatak>))]
    public async Task<ActionResult> ObrisiAdministratora(int id)
    {
        var administratorZaBrisanje = await _context.Administratori.FindAsync(id);
        _context.Administratori.Remove(administratorZaBrisanje!);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}