using Blanketomat.API.Context;
using Blanketomat.API.DTOs.KatedraDTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.KatedraFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class KatedraController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public KatedraController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("katedre")]
    [TypeFilter(typeof(ValidateDbSetFilter<Katedra>))]
    public ActionResult<IEnumerable<Katedra>> VratiSveKatedre()
    {
        return Ok(_context.Katedre);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Katedra>))]
    [TypeFilter(typeof(ValidateIdFilter<Katedra>))]
    public ActionResult<Katedra> VratiKatedru(int id)
    {
        // iz ValidateIdFilter-a
        return Ok(HttpContext.Items["entity"] as Katedra);
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDbSetFilter<Katedra>))]
    [TypeFilter(typeof(ValidateDodajKatedruFilter))]
    public async Task<ActionResult> DodajKatedru([FromBody] DodajKatedruDTO novaKatedra)
    {
        Katedra katedra = new Katedra
        {
            Naziv = novaKatedra.Naziv
        };

        if (novaKatedra.Smerovi != null)
        {
            Smer? smer;
            for (int i = 0; i < novaKatedra.Smerovi.Count(); i++)
            {
                smer = await _context.Smerovi.FindAsync(novaKatedra.Smerovi[i]);
                if (smer != null)
                {
                    if (!katedra.Smerovi!.Contains(smer))
                        katedra.Smerovi.Add(smer);
                }
            }
        }

        if (novaKatedra.Profesori != null)
        {
            Profesor? profesor;
            for (int i = 0; i < novaKatedra.Profesori.Count(); i++)
            {
                profesor = await _context.Profesori.FindAsync(novaKatedra.Profesori[i]);
                if (profesor != null)
                {
                    if (!katedra.Profesori!.Contains(profesor))
                        katedra.Profesori.Add(profesor);
                }
            }
        }

        if (novaKatedra.Asistenti != null)
        {
            Asistent? asistent;
            for (int i = 0; i < novaKatedra.Asistenti.Count(); i++)
            {
                asistent = await _context.Asistenti.FindAsync(novaKatedra.Asistenti[i]);
                if (asistent != null)
                {
                    if (!katedra.Asistenti!.Contains(asistent))
                        katedra.Asistenti.Add(asistent);
                }
            }
        }

        _context.Katedre.Add(katedra);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiKatedru),
            new { id = katedra.Id },
            katedra
            );
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Katedra>))]
    [TypeFilter(typeof(ValidateIdFilter<Katedra>))]
    public async Task<ActionResult<Katedra>> AzurirajKatedru(int id, [FromBody] AzurirajKatedruDTO katedra)
    {
        // iz ValidateIdFilter-a
        var katedraZaAzuriranje = HttpContext.Items["entity"] as Katedra;

        katedraZaAzuriranje!.Naziv = katedra.Naziv;

        if (katedra.Smerovi != null)
        {
            Smer? smer;
            for (int i = 0; i < katedra.Smerovi.Count(); i++)
            {
                smer = await _context.Smerovi.FindAsync(katedra.Smerovi[i]);
                if (smer != null)
                {
                    if (!katedraZaAzuriranje.Smerovi!.Contains(smer))
                        katedraZaAzuriranje.Smerovi.Add(smer);
                }
            }
        }

        if (katedra.Profesori != null)
        {
            Profesor? profesor;
            for (int i = 0; i < katedra.Profesori.Count(); i++)
            {
                profesor = await _context.Profesori.FindAsync(katedra.Profesori[i]);
                if (profesor != null)
                {
                    if (!katedraZaAzuriranje.Profesori!.Contains(profesor))
                        katedraZaAzuriranje.Profesori.Add(profesor);
                }
            }
        }

        if (katedra.Asistenti != null)
        {
            Asistent? asistent;
            for (int i = 0; i < katedra.Asistenti.Count(); i++)
            {
                asistent = await _context.Asistenti.FindAsync(katedra.Asistenti[i]);
                if (asistent != null)
                {
                    if (!katedraZaAzuriranje.Asistenti!.Contains(asistent))
                        katedraZaAzuriranje.Asistenti.Add(asistent);
                }
            }
        }

        await _context.SaveChangesAsync();
        return Ok(katedraZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Katedra>))]
    [TypeFilter(typeof(ValidateIdFilter<Katedra>))]
    public async Task<ActionResult<string>> ObrisiKatedru(int id)
    {
        // iz ValidateIdFilter-a
        var katedraZaBrisanje = HttpContext.Items["entity"] as Katedra;
        _context.Katedre.Remove(katedraZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Katedra uspešno izbrisana");
    }
}