using Blanketomat.API.Context;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.SlikaFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SlikaController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public SlikaController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Slika>))]
    [TypeFilter(typeof(ValidateIdFilter<Slika>))]
    public async Task<ActionResult> VratiSliku(int id)
    {
        return Ok(await _context.Slike.FindAsync(id));
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDodajSlikuFilter))]
    public async Task<ActionResult> DodajSliku([FromBody] Slika slika)
    {
        _context.Slike.Add(slika);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiSliku),
            new { id = slika.Id },
            slika
            );
    }

    [HttpPut]
    [TypeFilter(typeof(ValidateAzurirajSlikuFilter))]
    public async Task<ActionResult> AzurirajSliku([FromBody] Slika slika)
    {
        var slikaZaAzuriranje = HttpContext.Items["slika"] as Slika;
        slikaZaAzuriranje!.Putanja = slika.Putanja;
        slikaZaAzuriranje.Blanketi = slika.Blanketi;
        slikaZaAzuriranje.Komentar = slika.Komentar;

        await _context.SaveChangesAsync();
        return Ok(slikaZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Slika>))]
    [TypeFilter(typeof(ValidateIdFilter<Slika>))]
    public async Task<ActionResult> ObrisiSliku(int id)
    {
        var slikaZaBrisanje = await _context.Slike.FindAsync(id);
        _context.Slike.Remove(slikaZaBrisanje!);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}