using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PodoblastController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public PodoblastController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("{page}/{count}")]
    public async Task<ActionResult> VratiPodoblasti(int page, int count)
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
    [TypeFilter(typeof(ValidateIdFilter<Podoblast>))]
    public async Task<ActionResult> VratiPodoblast(int id)
    {
        return Ok(await _context.Podoblasti.FindAsync(id));
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDodajPodoblastFilter))]
    public async Task<ActionResult> DodajPodoblast([FromBody]Podoblast podoblast)
    {
        _context.Podoblasti.Add(podoblast);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Podoblast),
            new { id = podoblast.Id },
            podoblast
            );
    }

    [HttpPut]
    [TypeFilter(typeof(ValidateAzurirajPodoblastFilter))]
    public async Task<ActionResult> AzurirajPodoblast([FromBody]Podoblast podoblast)
    {
        var podoblastZaAzuriranje = HttpContext.Items["podoblast"] as Podoblast;

        podoblastZaAzuriranje!.Naziv = podoblast.Naziv;
        podoblastZaAzuriranje.Zadaci = podoblast.Zadaci;
        podoblastZaAzuriranje.Blanketi = podoblast.Blanketi;
        
        await _context.SaveChangesAsync();
        return Ok(podoblastZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Podoblast>))]
    public async Task<ActionResult> ObrisiPodoblast(int id)
    {
        var podoblastZaBrisanje = await _context.Podoblasti.FindAsync(id);
        _context.Podoblasti.Remove(podoblastZaBrisanje!);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}