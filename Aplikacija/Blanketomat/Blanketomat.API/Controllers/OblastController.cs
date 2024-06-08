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

    

        [HttpGet("{oblastId}/podoblasti")]
        public ActionResult<IEnumerable<Podoblast>> GetPodoblastiByOblast(int oblastId)
        {
            var oblast = _context.Oblasti
                .Include(o => o.Podoblasti)
                .FirstOrDefault(o => o.Id == oblastId);

            if (oblast == null)
            {
                return NotFound("Oblast nije pronađena.");
            }

            var podoblasti = oblast.Podoblasti;

            // Ako nema podoblasti, vratiti prazan niz umesto NotFound
            if (podoblasti == null)
            {
                return Ok(new List<Podoblast>());
            }

            return Ok(podoblasti);
        }

        [HttpPost("{oblastId}/podoblasti")]
        public ActionResult<Podoblast> CreatePodoblast(int oblastId, [FromBody] Podoblast newPodoblast)
        {
            var oblast = _context.Oblasti.FirstOrDefault(o => o.Id == oblastId);

            if (oblast == null)
            {
                return NotFound("Oblast nije pronađena.");
            }

            newPodoblast.Oblast = oblast;
            _context.Podoblasti.Add(newPodoblast);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetPodoblastiByOblast), new { oblastId = oblastId }, newPodoblast);
        }

    [HttpGet("oblasti")]
    [TypeFilter(typeof(ValidateDbSetFilter<Oblast>))]
    public ActionResult<IEnumerable<Oblast>> VratiSveOblast()
    {
        return Ok(_context.Oblasti);
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDbSetFilter<Oblast>))]
    [TypeFilter(typeof(ValidateDodajOblastFilter))]
    public async Task<ActionResult> DodajOblast([FromBody]DodajOblastDTO novaOblast)
    {
        Oblast oblast = new Oblast
        {
            Naziv = novaOblast.Naziv,
            Predmet = novaOblast.Predmet,
            Podoblasti = novaOblast.Podoblasti,
            Pitanja = novaOblast.Pitanja,
            Zadaci = novaOblast.Zadaci
        };

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
        oblastZaAzuriranje.Predmet = oblast.Predmet;
        oblastZaAzuriranje.Podoblasti = oblast.Podoblasti;
        oblastZaAzuriranje.Pitanja = oblast.Pitanja;
        oblastZaAzuriranje.Zadaci = oblast.Zadaci;

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

        return Ok("Oblast uspe�no obrisana");
    }
}