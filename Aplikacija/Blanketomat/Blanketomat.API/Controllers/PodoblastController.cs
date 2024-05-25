using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.DTOs.PodoblastDTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.PodoblastFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PodoblastController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public PodoblastController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Podoblast>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PaginationResponseDTO<Podoblast>>> VratiPodoblasti(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Podoblasti.Count() / (float)brojRezultata);

        var poblast = await _context.Podoblasti
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Podoblast>
        {
            Podaci = poblast,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Podoblast>))]
    [TypeFilter(typeof(ValidateIdFilter<Podoblast>))]
    public ActionResult<Podoblast> VratiPodoblast(int id)
    {
        // iz ValidateIdFilter-a
        return Ok(HttpContext.Items["entity"] as Podoblast);
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDbSetFilter<Podoblast>))]
    [TypeFilter(typeof(ValidateDodajPodoblastFilter))]
    public async Task<ActionResult> DodajPodoblast([FromBody]DodajPodoblastDTO novaPodoblast)
    {
        Podoblast podoblast = new Podoblast
        {
            Naziv = novaPodoblast.Naziv
        };

        if (novaPodoblast.OblastId != null)
        {
            Oblast? oblast = await _context.Oblasti.FindAsync(novaPodoblast.OblastId);
            if (oblast != null)
            {
                podoblast.Oblast = oblast;
            }
        }

        if (novaPodoblast.ZadaciIds != null)
        {
            Zadatak? zadatak;
            for (int i = 0; i < novaPodoblast.ZadaciIds.Count(); i++)
            {
                zadatak = await _context.Zadaci.FindAsync(novaPodoblast.ZadaciIds[i]);
                if (zadatak != null)
                {
                    if (!podoblast.Zadaci!.Contains(zadatak))
                        podoblast.Zadaci.Add(zadatak);
                }
            }
        }

        _context.Podoblasti.Add(podoblast);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Podoblast),
            new { id = podoblast.Id },
            podoblast
            );
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Podoblast>))]
    [TypeFilter(typeof(ValidateIdFilter<Podoblast>))]
    public async Task<ActionResult<Podoblast>> AzurirajPodoblast(int id, [FromBody]AzurirajPodoblastDTO podoblast)
    {
        // iz ValidateIdFilter-a
        var podoblastZaAzuriranje = HttpContext.Items["entity"] as Podoblast;

        podoblastZaAzuriranje!.Naziv = podoblast.Naziv;

        if (podoblast.OblastId != null)
        {
            Oblast? oblast = await _context.Oblasti.FindAsync(podoblast.OblastId);
            if (oblast != null)
            {
                podoblastZaAzuriranje.Oblast = oblast;
            }
        }

        if (podoblast.ZadaciIds != null)
        {
            Zadatak? zadatak;
            for (int i = 0; i < podoblast.ZadaciIds.Count(); i++)
            {
                zadatak = await _context.Zadaci.FindAsync(podoblast.ZadaciIds[i]);
                if (zadatak != null)
                {
                    if (!podoblastZaAzuriranje.Zadaci!.Contains(zadatak))
                        podoblastZaAzuriranje.Zadaci.Add(zadatak);
                }
            }
        }

        if (podoblast.BlanketiIds != null)
        {
            Blanket? blanket;
            for (int i = 0; i < podoblast.BlanketiIds.Count(); i++)
            {
                blanket = await _context.Blanketi.FindAsync(podoblast.BlanketiIds[i]);
                if (blanket != null)
                {
                    if (!podoblastZaAzuriranje.Blanketi!.Contains(blanket))
                        podoblastZaAzuriranje.Blanketi.Add(blanket);
                }
            }
        }

        await _context.SaveChangesAsync();
        return Ok(podoblastZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Podoblast>))]
    [TypeFilter(typeof(ValidateIdFilter<Podoblast>))]
    public async Task<ActionResult<string>> ObrisiPodoblast(int id)
    {
        // iz ValidateIdFilter-a
        var podoblastZaBrisanje = HttpContext.Items["entity"] as Podoblast;
        _context.Podoblasti.Remove(podoblastZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Podoblast uspešno obrisana");
    }
}