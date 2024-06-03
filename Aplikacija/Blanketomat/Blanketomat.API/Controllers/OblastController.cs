using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.DTOs.OblastDTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.OblastFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class OblastController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public OblastController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Oblast>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PagingResponseDTO<Oblast>>> VratiOblasti(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Oblasti.Count() / (float)brojRezultata);

        var oblasti = await _context.Oblasti
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PagingResponseDTO<Oblast>
        {
            Podaci = oblasti,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Oblast>))]
    [TypeFilter(typeof(ValidateIdFilter<Oblast>))]
    public ActionResult<Oblast> VratiOblast(int id)
    {
        // iz ValidateIdFilter-a
        return Ok(HttpContext.Items["entity"] as Oblast);
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDbSetFilter<Oblast>))]
    [TypeFilter(typeof(ValidateDodajOblastFilter))]
    public async Task<ActionResult> DodajOblast([FromBody]DodajOblastDTO novaOblast)
    {
        Oblast oblast = new Oblast
        {
            Naziv = novaOblast.Naziv
        };

        if (novaOblast.PredmetId != null)
        {
            Predmet? predmet = await _context.Predmeti.FindAsync(novaOblast.PredmetId);
            if (predmet != null)
            {
                oblast.Predmet = predmet;
            }
        }

        if (novaOblast.PodoblastiIds != null)
        {
            Podoblast? podoblast;
            for (int i = 0; i < novaOblast.PodoblastiIds.Count(); i++)
            {
                podoblast = await _context.Podoblasti.FindAsync(novaOblast.PodoblastiIds[i]);
                if(podoblast != null)
                {
                    if (!oblast.Podoblasti!.Contains(podoblast))
                        oblast.Podoblasti.Add(podoblast);
                }
            }
        }

        if (novaOblast.PitanjaIds != null)
        {
            Pitanje? pitanje;
            for (int i = 0; i < novaOblast.PitanjaIds.Count(); i++)
            {
                pitanje = await _context.Pitanja.FindAsync(novaOblast.PitanjaIds[i]);
                if (pitanje != null)
                {
                    if (!oblast.Pitanja!.Contains(pitanje))
                        oblast.Pitanja.Add(pitanje);
                }
            }
        }

        if (novaOblast.ZadaciIds != null)
        {
            Zadatak? zadatak;
            for (int i = 0; i < novaOblast.ZadaciIds.Count(); i++)
            {
                zadatak = await _context.Zadaci.FindAsync(novaOblast.ZadaciIds[i]);
                if (zadatak != null)
                {
                    if (!oblast.Zadaci!.Contains(zadatak))
                        oblast.Zadaci.Add(zadatak);
                }
            }
        }

        _context.Oblasti.Add(oblast);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiOblast),
            new { id = oblast.Id },
            oblast
            );
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Oblast>))]
    public async Task<ActionResult<Oblast>> AzurirajOblast(int id, [FromBody]AzurirajOblastDTO oblast)
    {
        // iz ValidateIdFilter-a
        var oblastZaAzuriranje = HttpContext.Items["entity"] as Oblast;
        oblastZaAzuriranje!.Naziv=oblast.Naziv;

        if (oblast.PredmetId != null)
        {
            Predmet? predmet = await _context.Predmeti.FindAsync(oblast.PredmetId);
            if (predmet != null)
            {
                oblastZaAzuriranje.Predmet = predmet;
            }
        }

        if (oblast.PodoblastiIds != null)
        {
            Podoblast? podoblast;
            for (int i = 0; i < oblast.PodoblastiIds.Count(); i++)
            {
                podoblast = await _context.Podoblasti.FindAsync(oblast.PodoblastiIds[i]);
                if (podoblast != null)
                {
                    if (!oblastZaAzuriranje.Podoblasti!.Contains(podoblast))
                        oblastZaAzuriranje.Podoblasti.Add(podoblast);
                }
            }
        }

        if (oblast.PitanjaIds != null)
        {
            Pitanje? pitanje;
            for (int i = 0; i < oblast.PitanjaIds.Count(); i++)
            {
                pitanje = await _context.Pitanja.FindAsync(oblast.PitanjaIds[i]);
                if (pitanje != null)
                {
                    if (!oblastZaAzuriranje.Pitanja!.Contains(pitanje))
                        oblastZaAzuriranje.Pitanja.Add(pitanje);
                }
            }
        }

        if (oblast.ZadaciIds != null)
        {
            Zadatak? zadatak;
            for (int i = 0; i < oblast.ZadaciIds.Count(); i++)
            {
                zadatak = await _context.Zadaci.FindAsync(oblast.ZadaciIds[i]);
                if (zadatak != null)
                {
                    if (!oblastZaAzuriranje.Zadaci!.Contains(zadatak))
                        oblastZaAzuriranje.Zadaci.Add(zadatak);
                }
            }
        }

        if (oblast.BlanketiIds != null)
        {
            Blanket? blanket;
            for (int i = 0; i < oblast.BlanketiIds.Count(); i++)
            {
                blanket = await _context.Blanketi.FindAsync(oblast.BlanketiIds[i]);
                if (blanket != null)
                {
                    if (!oblastZaAzuriranje.Blanketi!.Contains(blanket))
                        oblastZaAzuriranje.Blanketi.Add(blanket);
                }
            }
        }

        await _context.SaveChangesAsync();
        return Ok(oblastZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Oblast>))]
    [TypeFilter(typeof(ValidateIdFilter<Oblast>))]
    public async Task<ActionResult<string>> ObrisiOblast(int id)
    {
        var oblastZaBrisanje = HttpContext.Items["entity"] as Oblast;
        _context.Oblasti.Remove(oblastZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Oblast uspešno obrisana");
    }
}