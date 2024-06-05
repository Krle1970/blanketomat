using Blanketomat.API.Context;
using Blanketomat.API.DTOs.SlikaDTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.SlikaFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
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
    public ActionResult<Slika> VratiSliku(int id)
    {
        // iz ValidateIdFilter-a
        return Ok(HttpContext.Items["entity"] as Slika);
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDodajSlikuFilter))]
    public async Task<ActionResult> DodajSliku([FromBody] DodajSlikuDTO novaSlika)
    {
        Slika slika = new Slika
        {
            Putanja = novaSlika.Putanja,
            Blanketi = novaSlika.Blanketi,
            Pitanje = novaSlika.Pitanje,
            Zadatak = novaSlika.Zadatak,
            Komentar = novaSlika.Komentar,
            Odgovor = novaSlika.Odgovor
        };

        _context.Slike.Add(slika);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiSliku),
            new { id = slika.Id },
            slika
            );
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Slika>))]
    [TypeFilter(typeof(ValidateIdFilter<Slika>))]
    public async Task<ActionResult<Slika>> AzurirajSliku(int id, [FromBody] AzurirajSlikuDTO slika)
    {
        // iz ValidateIdFilter-a
        var slikaZaAzuriranje = HttpContext.Items["entity"] as Slika;
        slikaZaAzuriranje!.Putanja = slika.Putanja;
        slikaZaAzuriranje.Blanketi = slika.Blanketi;
        slikaZaAzuriranje.Pitanje = slika.Pitanje;
        slikaZaAzuriranje.Zadatak = slika.Zadatak;
        slikaZaAzuriranje.Komentar = slika.Komentar;
        slikaZaAzuriranje.Odgovor = slika.Odgovor;

        await _context.SaveChangesAsync();
        return Ok(slikaZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Slika>))]
    [TypeFilter(typeof(ValidateIdFilter<Slika>))]
    public async Task<ActionResult> ObrisiSliku(int id)
    {
        // iz ValidateIdFilter-a
        var slikaZaBrisanje = HttpContext.Items["entity"] as Slika;
        _context.Slike.Remove(slikaZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Slika uspešno izbrisana");
    }
}