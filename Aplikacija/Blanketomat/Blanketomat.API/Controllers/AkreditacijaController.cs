using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters.AkreditacijaFilters;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AkreditacijaController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public AkreditacijaController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult> VratiSveAkreditacije()
    {
        return Ok(await _context.Akreditacije.ToListAsync());
    }

    [HttpGet("{page}/{count}")]
    public async Task<ActionResult> VratiAkreditacije(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Akreditacije.Count() / (float)brojRezultata);

        var akreditacija = await _context.Akreditacije
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Akreditacija>
        {
            Podaci = akreditacija,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Akreditacija>))]
    public async Task<ActionResult> VratiAkreditaciju(int id)
    {
        return Ok(await _context.Akreditacije.FindAsync(id));
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDodajAkreditacijuFilter))]
    public async Task<ActionResult> DodajAkreditaciju([FromBody]Akreditacija akreditacija)
    {
        _context.Akreditacije.Add(akreditacija);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiAkreditaciju), 
            new { id = akreditacija.Id }, 
            akreditacija
            );
    }

    [HttpPut]
    [TypeFilter(typeof(ValidateAzurirajAkreditacijuFilter))]
    public async Task<ActionResult> AzurirajAkreditaciju([FromBody]Akreditacija akreditacija)
    {
        var akreditacijaZaAzuriranje = HttpContext.Items["akreditacija"] as Akreditacija;

        akreditacijaZaAzuriranje!.Naziv = akreditacija.Naziv;
        akreditacijaZaAzuriranje.Predmeti = akreditacija.Predmeti;
        akreditacijaZaAzuriranje.Studenti = akreditacija.Studenti;

        await _context.SaveChangesAsync();
        return Ok(akreditacijaZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Akreditacija>))]
    public async Task<ActionResult> ObrisiAkreditaciju(int id)
    {
        var akreditacijaZaBrisanje = await _context.Akreditacije.FindAsync(id);
        _context.Akreditacije.Remove(akreditacijaZaBrisanje!);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}