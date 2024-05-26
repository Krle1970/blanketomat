using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.DTOs.AsistentDTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Helper;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AsistentController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public AsistentController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("asistenti")]
    [TypeFilter(typeof(ValidateDbSetFilter<Asistent>))]
    public ActionResult<IEnumerable<Asistent>> VratiSveAsistente()
    {
        return Ok(_context.Asistenti);
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Asistent>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PaginationResponseDTO<Asistent>>> VratiAsistente(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Asistenti.Count() / (float)brojRezultata);

        var asistenti = await _context.Asistenti
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Asistent>
        {
            Podaci = asistenti,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };
        
        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Asistent>))]
    [TypeFilter(typeof(ValidateIdFilter<Asistent>))]
    public ActionResult<Asistent> VratiAsistenta(int id)
    {
        return Ok(HttpContext.Items["entity"] as Asistent);
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Asistent>))]
    [TypeFilter(typeof(ValidateIdFilter<Asistent>))]
    public async Task<ActionResult<Asistent>> AzurirajAsistenta(int id, [FromBody]AzurirajAsistentaDTO asistent)
    {
        // iz ValidateIdFilter-a
        var asistentZaAzuriranje = HttpContext.Items["entity"] as Asistent;

        PasswordManager.CreatePasswordHash(asistent.Password, out byte[] passwordHash, out byte[] passwordSalt);

        asistentZaAzuriranje!.Ime = asistent.Ime;
        asistentZaAzuriranje.Prezime = asistent.Prezime;
        asistentZaAzuriranje.Email = asistent.Email;
        asistentZaAzuriranje.PasswordHash = passwordHash;
        asistentZaAzuriranje.PasswordSalt = passwordSalt;

        if (asistent.KatedraId != null)
        {
            Katedra? katedra = await _context.Katedre.FindAsync(asistent.KatedraId);
            if (katedra != null)
            {
                asistentZaAzuriranje.Katedra = katedra;
            }
        }

        if (asistent.SmeroviIds != null)
        {
            Smer? smer;
            for (int i = 0; i < asistent.SmeroviIds.Length; i++)
            {
                smer = await _context.Smerovi.FindAsync(asistent.SmeroviIds[i]);
                if (smer != null)
                {
                    if (!asistentZaAzuriranje.Smerovi!.Contains(smer))
                        asistentZaAzuriranje.Smerovi.Add(smer);
                }
            }
        }

        if (asistent.PredmetiIds != null)
        {
            Predmet? predmet;
            for (int i = 0; i < asistent.PredmetiIds.Length; i++)
            {
                predmet = await _context.Predmeti.FindAsync(asistent.PredmetiIds[i]);
                if (predmet != null)
                {
                    if (!asistentZaAzuriranje.Predmeti!.Contains(predmet))
                        asistentZaAzuriranje.Predmeti.Add(predmet);
                }
            }
        }

        if (asistent.LajkovaniKomentariIds != null)
        {
            Komentar? komentar;
            for (int i = 0; i < asistent.LajkovaniKomentariIds.Length; i++)
            {
                komentar = await _context.Komentari.FindAsync(asistent.LajkovaniKomentariIds[i]);
                if (komentar != null)
                {
                    if (!asistentZaAzuriranje.LajkovaniKomentari!.Contains(komentar))
                        asistentZaAzuriranje.LajkovaniKomentari.Add(komentar);
                }
            }
        }

        if (asistent.LajkovaniOdgovoriIds != null)
        {
            Odgovor? odgovor;
            for (int i = 0; i < asistent.LajkovaniOdgovoriIds.Length; i++)
            {
                odgovor = await _context.Odgovori.FindAsync(asistent.LajkovaniOdgovoriIds[i]);
                if (odgovor != null)
                {
                    if (!asistentZaAzuriranje.LajkovaniOdgovori!.Contains(odgovor))
                        asistentZaAzuriranje.LajkovaniOdgovori.Add(odgovor);
                }
            }
        }

        await _context.SaveChangesAsync();
        return Ok(asistentZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Asistent>))]
    [TypeFilter(typeof(ValidateIdFilter<Asistent>))]
    public async Task<ActionResult<string>> ObrisiAsistenta(int id)
    {
        var asistentZaBrisanje = HttpContext.Items["entity"] as Asistent;
        _context.Asistenti.Remove(asistentZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Asistent uspešno obrisan");
    }
}