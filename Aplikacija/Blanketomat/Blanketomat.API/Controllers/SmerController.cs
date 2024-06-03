using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.DTOs.SmerDTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.SmerFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class SmerController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public SmerController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("smerovi")]
    [TypeFilter(typeof(ValidateDbSetFilter<Smer>))]
    public ActionResult<IEnumerable<Smer>> VratiSveSmerove()
    {
        return Ok(_context.Smerovi);
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Smer>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PagingResponseDTO<Smer>>> VratiSmerove(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Smerovi.Count() / (float)brojRezultata);

        var smerovi = await _context.Smerovi
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PagingResponseDTO<Smer>
        {
            Podaci = smerovi,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Smer>))]
    [TypeFilter(typeof(ValidateIdFilter<Smer>))]
    public ActionResult<Smer> VratiSmer(int id)
    {
        // iz ValidateIdFilter-a
        return Ok(HttpContext.Items["entity"] as Smer);
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDbSetFilter<Smer>))]
    [TypeFilter(typeof(ValidateDodajSmerFilter))]
    public async Task<ActionResult> DodajSmer([FromBody] DodajSmerDTO noviSmer)
    {
        Smer smer = new Smer
        {
            Naziv = noviSmer.Naziv
        };

        if (noviSmer.KatedraId != null)
        {
            Katedra? katedra = await _context.Katedre.FindAsync(noviSmer.KatedraId);
            if (katedra != null)
            {
                smer.Katedra = katedra;
            }
        }

        if (noviSmer.PredmetiIds != null)
        {
            Predmet? predmet;
            for (int i = 0; i < noviSmer.PredmetiIds.Count(); i++)
            {
                predmet = await _context.Predmeti.FindAsync(noviSmer.PredmetiIds[i]);
                if (predmet != null)
                {
                    if (!smer.Predmeti!.Contains(predmet))
                        smer.Predmeti.Add(predmet);
                }
            }
        }

        if (noviSmer.ProfesoriIds != null)
        {
            Profesor? profesor;
            for (int i = 0; i < noviSmer.ProfesoriIds.Count(); i++)
            {
                profesor = await _context.Profesori.FindAsync(noviSmer.ProfesoriIds[i]);
                if (profesor != null)
                {
                    if (!smer.Profesori!.Contains(profesor))
                        smer.Profesori.Add(profesor);
                }
            }
        }

        if (noviSmer.AsistentiIds != null)
        {
            Asistent? asistent;
            for (int i = 0; i < noviSmer.AsistentiIds.Count(); i++)
            {
                asistent = await _context.Asistenti.FindAsync(noviSmer.AsistentiIds[i]);
                if (asistent != null)
                {
                    if (!smer.Asistenti!.Contains(asistent))
                        smer.Asistenti.Add(asistent);
                }
            }
        }

        if (noviSmer.StudentiIds != null)
        {
            Student? student;
            for (int i = 0; i < noviSmer.StudentiIds.Count(); i++)
            {
                student = await _context.Studenti.FindAsync(noviSmer.StudentiIds[i]);
                if (student != null)
                {
                    if (!smer.Studenti!.Contains(student))
                        smer.Studenti.Add(student);
                }
            }
        }

        _context.Smerovi.Add(smer);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiSmer),
            new { id = smer.Id },
            smer
            );
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Smer>))]
    [TypeFilter(typeof(ValidateIdFilter<Smer>))]
    public async Task<ActionResult> AzurirajSmer(int id, [FromBody] AzurirajSmerDTO smer)
    {
        // iz ValidateIdFilter-a
        var smerZaAzuriranje = HttpContext.Items["entity"] as Smer;
        smerZaAzuriranje!.Naziv = smer.Naziv;

        if (smer.KatedraId != null)
        {
            Katedra? katedra = await _context.Katedre.FindAsync(smer.KatedraId);
            if (katedra != null)
            {
                smerZaAzuriranje.Katedra = katedra;
            }
        }

        if (smer.PredmetiIds != null)
        {
            Predmet? predmet;
            for (int i = 0; i < smer.PredmetiIds.Count(); i++)
            {
                predmet = await _context.Predmeti.FindAsync(smer.PredmetiIds[i]);
                if (predmet != null)
                {
                    if (!smerZaAzuriranje.Predmeti!.Contains(predmet))
                        smerZaAzuriranje.Predmeti.Add(predmet);
                }
            }
        }

        if (smer.ProfesoriIds != null)
        {
            Profesor? profesor;
            for (int i = 0; i < smer.ProfesoriIds.Count(); i++)
            {
                profesor = await _context.Profesori.FindAsync(smer.ProfesoriIds[i]);
                if (profesor != null)
                {
                    if (!smerZaAzuriranje.Profesori!.Contains(profesor))
                        smerZaAzuriranje.Profesori.Add(profesor);
                }
            }
        }

        if (smer.AsistentiIds != null)
        {
            Asistent? asistent;
            for (int i = 0; i < smer.AsistentiIds.Count(); i++)
            {
                asistent = await _context.Asistenti.FindAsync(smer.AsistentiIds[i]);
                if (asistent != null)
                {
                    if (!smerZaAzuriranje.Asistenti!.Contains(asistent))
                        smerZaAzuriranje.Asistenti.Add(asistent);
                }
            }
        }

        if (smer.StudentiIds != null)
        {
            Student? student;
            for (int i = 0; i < smer.StudentiIds.Count(); i++)
            {
                student = await _context.Studenti.FindAsync(smer.StudentiIds[i]);
                if (student != null)
                {
                    if (!smerZaAzuriranje.Studenti!.Contains(student))
                        smerZaAzuriranje.Studenti.Add(student);
                }
            }
        }

        await _context.SaveChangesAsync();
        return Ok(smerZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Smer>))]
    [TypeFilter(typeof(ValidateIdFilter<Smer>))]
    public async Task<ActionResult<string>> ObrisiSmer(int id)
    {
        // iz ValidateIdFilter-a
        var smerZaBrisanje = HttpContext.Items["entity"] as Smer;
        _context.Smerovi.Remove(smerZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Smer uspešno izbrisan");
    }
}