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
            Putanja = novaSlika.Putanja
        };

        if (novaSlika.BlanketiIds != null)
        {
            Blanket? blanket;
            for (int i = 0; i < novaSlika.BlanketiIds.Count(); i++)
            {
                blanket = await _context.Blanketi.FindAsync(novaSlika.BlanketiIds[i]);
                if (blanket != null)
                {
                    if (!slika.Blanketi!.Contains(blanket))
                        slika.Blanketi.Add(blanket);
                }
            }
        }

        if (novaSlika.PitanjeId != null)
        {
            Pitanje? pitanje = await _context.Pitanja.FindAsync(novaSlika.PitanjeId);
            if (pitanje != null)
            {
                slika.Pitanje = pitanje;
            }
        }

        if (novaSlika.ZadatakId != null)
        {
            Zadatak? zadatak = await _context.Zadaci.FindAsync(novaSlika.ZadatakId);
            if (zadatak != null)
            {
                slika.Zadatak = zadatak;
            }
        }

        if (novaSlika.KomentarId != null)
        {
            Komentar? komentar = await _context.Komentari.FindAsync(novaSlika.KomentarId);
            if (komentar != null)
            {
                slika.Komentar = komentar;
            }
        }

        if (novaSlika.OdgovorId != null)
        {
            Odgovor? odgovor = await _context.Odgovori.FindAsync(novaSlika.OdgovorId);
            if (odgovor != null)
            {
                slika.Odgovor = odgovor;
            }
        }

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

        if (slika.BlanketiIds != null)
        {
            Blanket? blanket;
            for (int i = 0; i < slika.BlanketiIds.Count(); i++)
            {
                blanket = await _context.Blanketi.FindAsync(slika.BlanketiIds[i]);
                if (blanket != null)
                {
                    if (!slikaZaAzuriranje.Blanketi!.Contains(blanket))
                        slikaZaAzuriranje.Blanketi.Add(blanket);
                }
            }
        }

        if (slika.PitanjeId != null)
        {
            Pitanje? pitanje = await _context.Pitanja.FindAsync(slika.PitanjeId);
            if (pitanje != null)
            {
                slikaZaAzuriranje.Pitanje = pitanje;
            }
        }

        if (slika.ZadatakId != null)
        {
            Zadatak? zadatak = await _context.Zadaci.FindAsync(slika.ZadatakId);
            if (zadatak != null)
            {
                slikaZaAzuriranje.Zadatak = zadatak;
            }
        }

        if (slika.KomentarId != null)
        {
            Komentar? komentar = await _context.Komentari.FindAsync(slika.KomentarId);
            if (komentar != null)
            {
                slikaZaAzuriranje.Komentar = komentar;
            }
        }

        if (slika.OdgovorId != null)
        {
            Odgovor? odgovor = await _context.Odgovori.FindAsync(slika.OdgovorId);
            if (odgovor != null)
            {
                slikaZaAzuriranje.Odgovor = odgovor;
            }
        }

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