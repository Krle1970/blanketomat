using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.KomentarFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blanketomat.API.DTOs.OdgovorDTOs;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class OdgovorController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public OdgovorController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Odgovor>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PagingResponseDTO<Odgovor>>> VratiOdgovore(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Odgovori.Count() / (float)brojRezultata);

        var odgovori = await _context.Odgovori
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PagingResponseDTO<Odgovor>
        {
            Podaci = odgovori,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Odgovor>))]
    [TypeFilter(typeof(ValidateIdFilter<Odgovor>))]
    public ActionResult<Odgovor> VratiOdgovor(int id)
    {
        // iz ValidateIdFilter-a
        return Ok(HttpContext.Items["entity"] as Odgovor);
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDbSetFilter<Odgovor>))]
    [TypeFilter(typeof(ValidateDodajKomentarFilter))]
    public async Task<ActionResult> DodajOdgovor([FromBody] DodajOdgovorDTO noviOdgovor)
    {
        Odgovor odgovor = new Odgovor
        {
            Tekst = noviOdgovor.Tekst,
            Lajkovi = 0
        };

        if (noviOdgovor.KomentarId != null)
        {
            Komentar? komentar = await _context.Komentari.FindAsync(noviOdgovor.KomentarId);
            if (komentar != null)
            {
                odgovor.Komentar = komentar;
            }
        }

        if (noviOdgovor.SlikeIds != null)
        {
            Slika? slika;
            for (int i = 0; i < noviOdgovor.SlikeIds.Count(); i++)
            {
                slika = await _context.Slike.FindAsync(noviOdgovor.SlikeIds[i]);
                if (slika != null)
                {
                    if (!odgovor.Slike!.Contains(slika))
                        odgovor.Slike.Add(slika);
                }
            }
        }

        if (noviOdgovor.StudentPostavioId != null)
        {
            Student? student = await _context.Studenti.FindAsync(noviOdgovor.StudentPostavioId);
            if (student != null)
            {
                odgovor.StudentPostavio = student;
            }
        }

        _context.Odgovori.Add(odgovor);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiOdgovor),
            new { id = odgovor.Id },
            odgovor
            );
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Odgovor>))]
    [TypeFilter(typeof(ValidateIdFilter<Odgovor>))]
    public async Task<ActionResult<Odgovor>> AzurirajOdgovor(int id, [FromBody] AzurirajOdgovorDTO odgovor)
    {
        // iz ValidateIdFilter-a
        var odgovorZaAzuriranje = HttpContext.Items["entity"] as Odgovor;

        odgovorZaAzuriranje!.Tekst = odgovor.Tekst;
        odgovorZaAzuriranje.Lajkovi = odgovor.Lajkovi;

        if (odgovor.KomentarId != null)
        {
            Komentar? komentar = await _context.Komentari.FindAsync(odgovor.KomentarId);
            if (komentar != null)
            {
                odgovorZaAzuriranje.Komentar = komentar;
            }
        }

        if (odgovor.SlikeIds != null)
        {
            Slika? slika;
            for (int i = 0; i < odgovor.SlikeIds.Count(); i++)
            {
                slika = await _context.Slike.FindAsync(odgovor.SlikeIds[i]);
                if (slika != null)
                {
                    if (!odgovorZaAzuriranje.Slike!.Contains(slika))
                        odgovorZaAzuriranje.Slike.Add(slika);
                }
            }
        }

        if (odgovor.StudentPostavioId != null)
        {
            Student? student = await _context.Studenti.FindAsync(odgovor.StudentPostavioId);
            if (student != null)
            {
                odgovorZaAzuriranje.StudentPostavio = student;
            }
        }

        if (odgovor.AsistentiLikedIds != null)
        {
            Asistent? asistent;
            for (int i = 0; i < odgovor.AsistentiLikedIds.Count(); i++)
            {
                asistent = await _context.Asistenti.FindAsync(odgovor.AsistentiLikedIds[i]);
                if (asistent != null)
                {
                    if (!odgovorZaAzuriranje.AsistentiVerifikovali!.Contains(asistent))
                        odgovorZaAzuriranje.AsistentiVerifikovali.Add(asistent);
                }
            }
        }

        if (odgovor.ProfesoriLikedIds != null)
        {
            Profesor? profesor;
            for (int i = 0; i < odgovor.ProfesoriLikedIds.Count(); i++)
            {
                profesor = await _context.Profesori.FindAsync(odgovor.ProfesoriLikedIds[i]);
                if (profesor != null)
                {
                    if (!odgovorZaAzuriranje.ProfesoriVerifikovali!.Contains(profesor))
                        odgovorZaAzuriranje.ProfesoriVerifikovali.Add(profesor);
                }
            }
        }

        await _context.SaveChangesAsync();
        return Ok(odgovorZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Odgovor>))]
    [TypeFilter(typeof(ValidateIdFilter<Odgovor>))]
    public async Task<ActionResult> ObrisiKomentar(int id)
    {
        // iz ValidateIdFilter-a
        var odgovorZaBrisanje = HttpContext.Items["entity"] as Odgovor;
        _context.Odgovori.Remove(odgovorZaBrisanje!);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}