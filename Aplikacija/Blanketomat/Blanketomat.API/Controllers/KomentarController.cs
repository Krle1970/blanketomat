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
            Lajkovi = 0
        };

        if (noviKomentar.BlanketId != null)
        {
            Blanket? blanket = await _context.Blanketi.FindAsync(noviKomentar.BlanketId);
            if (blanket != null)
            {
                komentar.Blanket = blanket;
            }
        }

        if (noviKomentar.SlikeIds != null)
        {
            Slika? slika;
            for (int i = 0; i < noviKomentar.SlikeIds.Count(); i++)
            {
                slika = await _context.Slike.FindAsync(noviKomentar.SlikeIds[i]);
                if (slika != null)
                {
                    if (!komentar.Slika!.Contains(slika))
                        komentar.Slika.Add(slika);
                }
            }
        }

        if (noviKomentar.StudentPostavioId != null)
        {
            Student? student = await _context.Studenti.FindAsync(noviKomentar.StudentPostavioId);
            if (student != null)
            {
                komentar.StudentPostavio = student;
            }
        }

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

        if (komentar.BlanketId != null)
        {
            Blanket? blanket = await _context.Blanketi.FindAsync(komentar.BlanketId);
            if (blanket != null)
            {
                komentarZaAzuriranje.Blanket = blanket;
            }
        }

        if (komentar.SlikeIds != null)
        {
            Slika? slika;
            for (int i = 0; i < komentar.SlikeIds.Count(); i++)
            {
                slika = await _context.Slike.FindAsync(komentar.SlikeIds[i]);
                if (slika != null)
                {
                    if (!komentarZaAzuriranje.Slika!.Contains(slika))
                        komentarZaAzuriranje.Slika.Add(slika);
                }
            }
        }

        if (komentar.StudentPostavioId != null)
        {
            Student? student = await _context.Studenti.FindAsync(komentar.StudentPostavioId);
            if (student != null)
            {
                komentarZaAzuriranje.StudentPostavio = student;
            }
        }

        if (komentar.AsistentiLikedIds != null)
        {
            Asistent? asistent;
            for (int i = 0; i < komentar.AsistentiLikedIds.Count(); i++)
            {
                asistent = await _context.Asistenti.FindAsync(komentar.AsistentiLikedIds[i]);
                if (asistent != null)
                {
                    if (!komentarZaAzuriranje.AsistentiLiked!.Contains(asistent))
                        komentarZaAzuriranje.AsistentiLiked.Add(asistent);
                }
            }
        }

        if (komentar.ProfesoriLikedIds != null)
        {
            Profesor? profesor;
            for (int i = 0; i < komentar.ProfesoriLikedIds.Count(); i++)
            {
                profesor = await _context.Profesori.FindAsync(komentar.ProfesoriLikedIds[i]);
                if (profesor != null)
                {
                    if (!komentarZaAzuriranje.ProfesoriLiked!.Contains(profesor))
                        komentarZaAzuriranje.ProfesoriLiked.Add(profesor);
                }
            }
        }

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