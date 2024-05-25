using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters.AkreditacijaFilters;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AkreditacijaController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public AkreditacijaController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("akreditacije")]
    [TypeFilter(typeof(ValidateDbSetFilter<Akreditacija>))]
    public async Task<ActionResult<List<Akreditacija>>> VratiSveAkreditacije()
    {
        return Ok(await _context.Akreditacije.ToListAsync());
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Akreditacija>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PaginationResponseDTO<Akreditacija>>> VratiAkreditacije(int page, int count)
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
    [TypeFilter(typeof(ValidateDbSetFilter<Akreditacija>))]
    [TypeFilter(typeof(ValidateIdFilter<Akreditacija>))]
    public ActionResult<Akreditacija> VratiAkreditaciju(int id)
    {
        // iz ValidateIdFilter-a
        return Ok(HttpContext.Items["entity"] as Akreditacija);
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDbSetFilter<Akreditacija>))]
    [TypeFilter(typeof(ValidateDodajAkreditacijuFilter))]
    public async Task<ActionResult> DodajAkreditaciju([FromBody]AkreditacijaDTO novaAkreditacija)
    {
        Akreditacija akreditacija = new Akreditacija
        {
            Naziv = novaAkreditacija.Naziv,
            Studenti = new List<Student>(),
            Predmeti = new List<Predmet>()
        };

        if (novaAkreditacija!.StudentiIds != null)
        {
            Student? student;
            for (int i = 0; i < novaAkreditacija.StudentiIds.Count(); i++)
            {
                student = await _context.Studenti.FindAsync(novaAkreditacija.StudentiIds[i]);
                if (student != null)
                {
                    if (!akreditacija.Studenti.Contains(student))
                        akreditacija.Studenti.Add(student);
                }
            }
        }

        if (novaAkreditacija.PredmetiIds != null)
        {
            Predmet? predmet;
            for (int i = 0; i < novaAkreditacija.PredmetiIds.Count(); i++)
            {
                predmet = await _context.Predmeti.FindAsync(novaAkreditacija.PredmetiIds[i]);
                if (predmet != null)
                {
                    if (!akreditacija.Predmeti.Contains(predmet))
                        akreditacija.Predmeti.Add(predmet);
                }
            }
        }

        _context.Akreditacije.Add(akreditacija);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiAkreditaciju), 
            new { id = akreditacija.Id },
            akreditacija
            );
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Akreditacija>))]
    [TypeFilter(typeof(ValidateIdFilter<Akreditacija>))]
    public async Task<ActionResult> AzurirajAkreditaciju(int id, [FromBody]AkreditacijaDTO akreditacija)
    {
        // iz ValidateIdFilter-a
        var akreditacijaZaAzuriranje = HttpContext.Items["entity"] as Akreditacija;

        akreditacijaZaAzuriranje!.Naziv = akreditacija.Naziv;

        if (akreditacija!.StudentiIds != null)
        {
            Student? student;
            for (int i = 0; i < akreditacija.StudentiIds.Count(); i++)
            {
                student = await _context.Studenti.FindAsync(akreditacija.StudentiIds[i]);
                if (student != null)
                {
                    if (!akreditacijaZaAzuriranje.Studenti!.Contains(student))
                        akreditacijaZaAzuriranje.Studenti.Add(student);
                }
            }
        }

        if (akreditacija.PredmetiIds != null)
        {
            Predmet? predmet;
            for (int i = 0; i < akreditacija.PredmetiIds.Count(); i++)
            {
                predmet = await _context.Predmeti.FindAsync(akreditacija.PredmetiIds[i]);
                if (predmet != null)
                {
                    if (!akreditacijaZaAzuriranje.Predmeti!.Contains(predmet))
                        akreditacijaZaAzuriranje.Predmeti.Add(predmet);
                }
            }
        }

        await _context.SaveChangesAsync();
        return Ok(akreditacijaZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Akreditacija>))]
    [TypeFilter(typeof(ValidateIdFilter<Akreditacija>))]
    public async Task<ActionResult> ObrisiAkreditaciju(int id)
    {
        // iz ValidateIdFilter-a
        var akreditacijaZaBrisanje = HttpContext.Items["entity"] as Akreditacija;
        _context.Akreditacije.Remove(akreditacijaZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Akreditacija uspešno obrisana");
    }
}