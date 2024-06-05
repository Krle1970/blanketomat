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
            Lajkovi = 0,
            Komentar = noviOdgovor.Komentar,
            Slike = noviOdgovor.Slike,
            StudentPostavio = noviOdgovor.StudentPostavio
        };

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
        odgovorZaAzuriranje.Komentar = odgovor.Komentar;
        odgovorZaAzuriranje.Slike = odgovor.Slike;
        odgovorZaAzuriranje.StudentPostavio = odgovor.StudentPostavio;
        odgovorZaAzuriranje.ProfesoriVerifikovali = odgovor.ProfesoriLiked;
        odgovorZaAzuriranje.AsistentiVerifikovali = odgovor.AsistentiLiked;

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