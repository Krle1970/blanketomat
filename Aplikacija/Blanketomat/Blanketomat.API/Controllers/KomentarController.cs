using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.DTOs.KomentarDTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.KomentarFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class KomentarController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public KomentarController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Komentar>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PagingResponseDTO<Komentar>>> VratiKomentare(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Komentari.Count() / (float)brojRezultata);

        var komentari = await _context.Komentari
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PagingResponseDTO<Komentar>
        {
            Podaci = komentari,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Komentar>))]
    [TypeFilter(typeof(ValidateIdFilter<Komentar>))]
    public ActionResult<Komentar> VratiKomentar(int id)
    {
        // iz ValidateIdFilter-a
        return Ok(HttpContext.Items["entity"] as Komentar);
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDbSetFilter<Komentar>))]
    [TypeFilter(typeof(ValidateDodajKomentarFilter))]
    public async Task<ActionResult> DodajKomentar([FromBody]DodajKomentarDTO noviKomentar)
    {
        Komentar komentar = new Komentar
        {
            Tekst = noviKomentar.Tekst,
            Lajkovi = 0,
            Blanket = noviKomentar.Blanket,
            Slika = noviKomentar.Slike,
            StudentPostavio = noviKomentar.StudentPostavio
        };

        _context.Komentari.Add(komentar);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiKomentar),
            new { id = komentar.Id },
            komentar
            );
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Komentar>))]
    [TypeFilter(typeof(ValidateIdFilter<Komentar>))]
    public async Task<ActionResult<Komentar>> AzurirajKomentar(int id, [FromBody]AzurirajKomentarDTO komentar)
    {
        // iz ValidateIdFilter-a
        var komentarZaAzuriranje = HttpContext.Items["entity"] as Komentar;

        komentarZaAzuriranje!.Tekst = komentar.Tekst;
        komentarZaAzuriranje.Lajkovi = komentar.Lajkovi;
        komentarZaAzuriranje.Blanket = komentar.Blanket;
        komentarZaAzuriranje.Slika = komentar.Slike;
        komentarZaAzuriranje.StudentPostavio = komentar.StudentPostavio;
        komentarZaAzuriranje.Odgovori = komentar.Odgovori;
        komentarZaAzuriranje.ProfesoriLiked = komentar.ProfesoriLiked;
        komentarZaAzuriranje.AsistentiLiked = komentar.AsistentiLiked;

        await _context.SaveChangesAsync();
        return Ok(komentarZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Komentar>))]
    [TypeFilter(typeof(ValidateIdFilter<Komentar>))]
    public async Task<ActionResult<string>> ObrisiKomentar(int id)
    {
        // iz ValidateIdFilter-a
        var KomentarZaBrisanje = HttpContext.Items["entity"] as Komentar;
        _context.Komentari.Remove(KomentarZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Komentar uspešno obrisan");
    }
}