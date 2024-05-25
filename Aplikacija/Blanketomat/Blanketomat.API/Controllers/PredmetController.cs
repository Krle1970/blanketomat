using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.DTOs.PredmetDTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.PredmetFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PredmetController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public PredmetController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Predmet>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PaginationResponseDTO<Predmet>>> VratiPredmete(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Predmeti.Count() / (float)brojRezultata);

        var predmeti = await _context.Predmeti
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Predmet>
        {
            Podaci = predmeti,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Predmet>))]
    [TypeFilter(typeof(ValidateIdFilter<Predmet>))]
    public ActionResult<Predmet> VratiPredmet(int id)
    {
        // iz ValidateIdFilter-a
        return Ok(HttpContext.Items["entity"] as Predmet);
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDbSetFilter<Predmet>))]
    [TypeFilter(typeof(ValidateDodajPredmetFilter))]
    public async Task<ActionResult> DodajPredmet([FromBody]DodajPredmetDTO noviPredmet)
    {
        Predmet predmet = new Predmet
        {
            Naziv = noviPredmet.Naziv,
            Godina = noviPredmet.Godina
        };

        if (noviPredmet.AkreditacijaId != null)
        {
            Akreditacija? akreditacija = await _context.Akreditacije.FindAsync(noviPredmet.AkreditacijaId);
            if (akreditacija != null)
            {
                predmet.Akreditacija = akreditacija;
            }
        }

        if (noviPredmet.OblastiIds != null)
        {
            Oblast? oblast;
            for (int i = 0; i < noviPredmet.OblastiIds.Count(); i++)
            {
                oblast = await _context.Oblasti.FindAsync(noviPredmet.OblastiIds[i]);
                if (oblast != null)
                {
                    if (!predmet.Oblasti!.Contains(oblast))
                        predmet.Oblasti.Add(oblast);
                }
            }
        }

        if (noviPredmet.SmerId != null)
        {
            Smer? smer = await _context.Smerovi.FindAsync(noviPredmet.SmerId);
            if (smer != null)
            {
                predmet.Smer = smer;
            }
        }

        if (noviPredmet.ProfesoriIds != null)
        {
            Profesor? profesor;
            for (int i = 0; i < noviPredmet.ProfesoriIds.Count(); i++)
            {
                profesor = await _context.Profesori.FindAsync(noviPredmet.ProfesoriIds[i]);
                if (profesor != null)
                {
                    if (!predmet.Profesori!.Contains(profesor))
                        predmet.Profesori.Add(profesor);
                }
            }
        }

        if (noviPredmet.AsistentiIds != null)
        {
            Asistent? asistent;
            for (int i = 0; i < noviPredmet.AsistentiIds.Count(); i++)
            {
                asistent = await _context.Asistenti.FindAsync(noviPredmet.AsistentiIds[i]);
                if (asistent != null)
                {
                    if (!predmet.Asistenti!.Contains(asistent))
                        predmet.Asistenti.Add(asistent);
                }
            }
        }

        if (noviPredmet.StudentiIds != null)
        {
            Student? student;
            for (int i = 0; i < noviPredmet.StudentiIds.Count(); i++)
            {
                student = await _context.Studenti.FindAsync(noviPredmet.StudentiIds[i]);
                if (student != null)
                {
                    if (!predmet.Studenti!.Contains(student))
                        predmet.Studenti.Add(student);
                }
            }
        }

        _context.Predmeti.Add(predmet);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiPredmet), 
            new { id = predmet.Id }, 
            predmet
            );
    }

    [HttpPut]
    [TypeFilter(typeof(ValidateDbSetFilter<Predmet>))]
    [TypeFilter(typeof(ValidateIdFilter<Predmet>))]
    public async Task<ActionResult<Predmet>> AzurirajPredmet(int id, [FromBody]AzurirajPredmetDTO predmet)
    {
        // iz ValidateIdFilter-a
        var predmetZaAzuriranje = HttpContext.Items["entity"] as Predmet;
        predmetZaAzuriranje!.Naziv = predmet.Naziv;
        predmetZaAzuriranje.Godina = predmet.Godina;

        if (predmet.AkreditacijaId != null)
        {
            Akreditacija? akreditacija = await _context.Akreditacije.FindAsync(predmet.AkreditacijaId);
            if (akreditacija != null)
            {
                predmetZaAzuriranje.Akreditacija = akreditacija;
            }
        }

        if (predmet.OblastiIds != null)
        {
            Oblast? oblast;
            for (int i = 0; i < predmet.OblastiIds.Count(); i++)
            {
                oblast = await _context.Oblasti.FindAsync(predmet.OblastiIds[i]);
                if (oblast != null)
                {
                    if (!predmetZaAzuriranje.Oblasti!.Contains(oblast))
                        predmetZaAzuriranje.Oblasti.Add(oblast);
                }
            }
        }

        if (predmet.BlanketiIds != null)
        {
            Blanket? blanket;
            for (int i = 0; i < predmet.BlanketiIds.Count(); i++)
            {
                blanket = await _context.Blanketi.FindAsync(predmet.BlanketiIds[i]);
                if (blanket != null)
                {
                    if (!predmetZaAzuriranje.Blanketi!.Contains(blanket))
                        predmetZaAzuriranje.Blanketi.Add(blanket);
                }
            }
        }

        if (predmet.SmerId != null)
        {
            Smer? smer = await _context.Smerovi.FindAsync(predmet.SmerId);
            if (smer != null)
            {
                predmetZaAzuriranje.Smer = smer;
            }
        }

        if (predmet.ProfesoriIds != null)
        {
            Profesor? profesor;
            for (int i = 0; i < predmet.ProfesoriIds.Count(); i++)
            {
                profesor = await _context.Profesori.FindAsync(predmet.ProfesoriIds[i]);
                if (profesor != null)
                {
                    if (!predmetZaAzuriranje.Profesori!.Contains(profesor))
                        predmetZaAzuriranje.Profesori.Add(profesor);
                }
            }
        }

        if (predmet.AsistentiIds != null)
        {
            Asistent? asistent;
            for (int i = 0; i < predmet.AsistentiIds.Count(); i++)
            {
                asistent = await _context.Asistenti.FindAsync(predmet.AsistentiIds[i]);
                if (asistent != null)
                {
                    if (!predmetZaAzuriranje.Asistenti!.Contains(asistent))
                        predmetZaAzuriranje.Asistenti.Add(asistent);
                }
            }
        }

        if (predmet.StudentiIds != null)
        {
            Student? student;
            for (int i = 0; i < predmet.StudentiIds.Count(); i++)
            {
                student = await _context.Studenti.FindAsync(predmet.StudentiIds[i]);
                if (student != null)
                {
                    if (!predmetZaAzuriranje.Studenti!.Contains(student))
                        predmetZaAzuriranje.Studenti.Add(student);
                }
            }
        }

        await _context.SaveChangesAsync();
        return Ok(predmetZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Predmet>))]
    [TypeFilter(typeof(ValidateIdFilter<Predmet>))]
    public async Task<ActionResult<string>> ObrisiPredmet(int id)
    {
        // iz ValidateIdFilter-a
        var predmetZaBrisanje = HttpContext.Items["entity"] as Predmet;
        _context.Predmeti.Remove(predmetZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Predmet uspešno obrisan");
    }
}