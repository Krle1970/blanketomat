using Blanketomat.API.Context;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.KatedraFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KatedraController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public KatedraController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Katedra>))]
    public async Task<ActionResult> VratiSveKatedre()
    {
        return Ok(await _context.Katedre.ToListAsync());
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Katedra>))]
    [TypeFilter(typeof(ValidateIdFilter<Katedra>))]
    public async Task<ActionResult> VratiKatedru(int id)
    {
        return Ok(await _context.Katedre.FindAsync(id));
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDodajKatedruFilter))]
    public async Task<ActionResult> DodajKatedru([FromBody] Katedra katedra)
    {
        _context.Katedre.Add(katedra);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiKatedru),
            new { id = katedra.Id },
            katedra
            );
    }

    [HttpPut]
    [TypeFilter(typeof(ValidateAzurirajKatedruFilter))]
    public async Task<ActionResult> AzurirajKatedru([FromBody] Katedra katedra)
    {
        var katedraZaAzuriranje = HttpContext.Items["katedra"] as Katedra;
        katedraZaAzuriranje!.Naziv = katedra.Naziv;
        katedraZaAzuriranje.Smerovi = katedra.Smerovi;
        //katedraZaAzuriranje.Predmeti = katedra.Predmeti;
        katedraZaAzuriranje.Profesori = katedra.Profesori;
        katedraZaAzuriranje.Asistenti = katedra.Asistenti;

        await _context.SaveChangesAsync();
        return Ok(katedraZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Katedra>))]
    [TypeFilter(typeof(ValidateIdFilter<Katedra>))]
    public async Task<ActionResult> ObrisiKatedru(int id)
    {
        var katedraZaBrisanje = await _context.Katedre.FindAsync(id);
        _context.Katedre.Remove(katedraZaBrisanje!);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}