using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.DTOs.ZadatakDTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.ZadatakFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class ZadatakController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public ZadatakController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Zadatak>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PaginationResponseDTO<Zadatak>>> VratiZadatke(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Zadaci.Count() / (float)brojRezultata);

        var zadaci = await _context.Zadaci
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Zadatak>
        {
            Podaci = zadaci,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Zadatak>))]
    [TypeFilter(typeof(ValidateIdFilter<Zadatak>))]
    public ActionResult<Zadatak> VratiZadatak(int id)
    {
        // iz ValidateIdFilter-a
        return Ok(HttpContext.Items["entity"] as Zadatak);
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDbSetFilter<Zadatak>))]
    [TypeFilter(typeof(ValidateDodajZadatakFilter))]
    public async Task<ActionResult> DodajZadatak([FromBody] DodajZadatakDTO noviZadatak)
    {
        Zadatak zadatak = new Zadatak
        {
            Tekst = noviZadatak.Tekst
        };

        if (noviZadatak.SlikeIds != null)
        {
            Slika? slika;
            for (int i = 0; i < noviZadatak.SlikeIds.Count(); i++)
            {
                slika = await _context.Slike.FindAsync(noviZadatak.SlikeIds[i]);
                if (slika != null)
                {
                    if (!zadatak.Slika!.Contains(slika))
                        zadatak.Slika.Add(slika);
                }
            }
        }

        if (noviZadatak.OblastId != null)
        {
            Oblast? oblast = await _context.Oblasti.FindAsync(noviZadatak.OblastId);
            if (oblast != null)
            {
                zadatak.Oblast = oblast;
            }
        }

        if (noviZadatak.PodoblastiIds != null)
        {
            Podoblast? podoblast;
            for (int i = 0; i < noviZadatak.PodoblastiIds.Count(); i++)
            {
                podoblast = await _context.Podoblasti.FindAsync(noviZadatak.PodoblastiIds[i]);
                if (podoblast != null)
                {
                    if (!zadatak.Podoblast!.Contains(podoblast))
                        zadatak.Podoblast.Add(podoblast);
                }
            }
        }

        if (noviZadatak.BlanketiIds != null)
        {
            Blanket? blanket;
            for (int i = 0; i < noviZadatak.BlanketiIds.Count(); i++)
            {
                blanket = await _context.Blanketi.FindAsync(noviZadatak.BlanketiIds[i]);
                if (blanket != null)
                {
                    if (!zadatak.Blanketi!.Contains(blanket))
                        zadatak.Blanketi.Add(blanket);
                }
            }
        }

        _context.Zadaci.Add(zadatak);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiZadatak),
            new { id = zadatak.Id },
            zadatak
            );
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Zadatak>))]
    [TypeFilter(typeof(ValidateIdFilter<Zadatak>))]
    public async Task<ActionResult<Zadatak>> AzurirajZadatak(int id, [FromBody] AzurirajZadatakDTO zadatak)
    {
        // iz ValidateIdFilter-a
        var zadatakZaAzuriranje = HttpContext.Items["entity"] as Zadatak;
        zadatakZaAzuriranje!.Tekst = zadatak.Tekst;

        if (zadatak.SlikeIds != null)
        {
            Slika? slika;
            for (int i = 0; i < zadatak.SlikeIds.Count(); i++)
            {
                slika = await _context.Slike.FindAsync(zadatak.SlikeIds[i]);
                if (slika != null)
                {
                    if (!zadatakZaAzuriranje!.Slika!.Contains(slika))
                        zadatakZaAzuriranje.Slika.Add(slika);
                }
            }
        }

        if (zadatak.OblastId != null)
        {
            Oblast? oblast = await _context.Oblasti.FindAsync(zadatak.OblastId);
            if (oblast != null)
            {
                zadatakZaAzuriranje!.Oblast = oblast;
            }
        }

        if (zadatak.PodoblastiIds != null)
        {
            Podoblast? podoblast;
            for (int i = 0; i < zadatak.PodoblastiIds.Count(); i++)
            {
                podoblast = await _context.Podoblasti.FindAsync(zadatak.PodoblastiIds[i]);
                if (podoblast != null)
                {
                    if (!zadatakZaAzuriranje!.Podoblast!.Contains(podoblast))
                        zadatakZaAzuriranje.Podoblast.Add(podoblast);
                }
            }
        }

        if (zadatak.BlanketiIds != null)
        {
            Blanket? blanket;
            for (int i = 0; i < zadatak.BlanketiIds.Count(); i++)
            {
                blanket = await _context.Blanketi.FindAsync(zadatak.BlanketiIds[i]);
                if (blanket != null)
                {
                    if (!zadatakZaAzuriranje!.Blanketi!.Contains(blanket))
                        zadatakZaAzuriranje.Blanketi.Add(blanket);
                }
            }
        }

        await _context.SaveChangesAsync();
        return Ok(zadatakZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Zadatak>))]
    [TypeFilter(typeof(ValidateIdFilter<Zadatak>))]
    public async Task<ActionResult<string>> ObrisiZadatak(int id)
    {
        var zadatakZaBrisanje = HttpContext.Items["entity"] as Zadatak;
        _context.Zadaci.Remove(zadatakZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Zadatak uspešno obrisan");
    }
}