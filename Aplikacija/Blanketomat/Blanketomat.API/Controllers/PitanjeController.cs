using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.DTOs.PitanjeDTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.PitanjeFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PitanjeController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public PitanjeController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Pitanje>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PagingResponseDTO<Pitanje>>> VratiPitanja(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Pitanja.Count() / (float)brojRezultata);

        var pitanja = await _context.Pitanja
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PagingResponseDTO<Pitanje>
        {
            Podaci = pitanja,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Pitanje>))]
    [TypeFilter(typeof(ValidateIdFilter<Pitanje>))]
    public ActionResult<Pitanje> VratiPitanje(int id)
    {
        // iz ValidateIdFilter-a
        return Ok(HttpContext.Items["entity"] as Pitanje);
    }

   
    

        [HttpGet("VratiPitanje{id}")]
        public ActionResult<PitanjeDTO> GetPitanjeById(int id)
        {
            var pitanje = _context.Pitanja
                .Include(p => p.Slika)
                .Include(p => p.Oblast)
                .FirstOrDefault(p => p.Id == id);

            if (pitanje == null)
            {
                return NotFound("Pitanje nije pronađeno.");
            }

            var pitanjeDto = new PitanjeDTO
            {
                Tekst = pitanje.Tekst,
                Oblast = pitanje.Oblast,
                Slike = pitanje.Slika
            };

            return Ok(pitanjeDto);
        }
    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Pitanje>))]
    public async Task<ActionResult<Pitanje>> AzurirajPitanje(int id, [FromBody]AzurirajPitanjeDTO pitanje)
    {
        // iz ValidateIdFilter-a
        var pitanjeZaAzuriranje = HttpContext.Items["entity"] as Pitanje;

        pitanjeZaAzuriranje!.Tekst = pitanje.Tekst;
        pitanjeZaAzuriranje.Slika = pitanje.Slike;
        pitanjeZaAzuriranje.Oblast = pitanje.Oblast;
        pitanjeZaAzuriranje.Blanketi = pitanje.Blanketi;

        await _context.SaveChangesAsync();
        return Ok(pitanjeZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Pitanje>))]
    [TypeFilter(typeof(ValidateIdFilter<Pitanje>))]
    public async Task<ActionResult<string>> ObrisiPitanje(int id)
    {
        // iz ValidateIdFilter-a
        var pitanjeZaBrisanje = HttpContext.Items["entity"] as Pitanje;
        _context.Pitanja.Remove(pitanjeZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Pitanje uspe�no obrisano");
    }
   [HttpPost("dodajPitanje")]
    public async Task<IActionResult> DodajPitanje([FromBody] PitanjeDTO pitanjeDTO)
    {
        if (!ModelState.IsValid)
        {
            var errorList = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { Errors = errorList });
        }

        var pitanje = new Pitanje
        {
            Tekst = pitanjeDTO.Tekst,
            Oblast = pitanjeDTO.Oblast != null ? await _context.Oblasti.FindAsync(pitanjeDTO.Oblast.Id) : null,
            Slika = pitanjeDTO.Slike?.ToList()
        };

        _context.Pitanja.Add(pitanje);
        await _context.SaveChangesAsync();

        return Ok(pitanje);
    }
    [HttpGet("SvePitanja")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllPitanja()
        {
            var pitanja = await _context.Pitanja
                .Select(p => new
                {
                    p.Id,
                    p.Tekst
                })
                .ToListAsync();

            if (pitanja == null || !pitanja.Any())
            {
                return NotFound("Nije pronađeno nijedno pitanje.");
            }

            return Ok(pitanja);
        }
[HttpGet("Oblast/{oblastId?}")]
public async Task<ActionResult<IEnumerable<object>>> GetPitanjaByOblast(int? oblastId)
{
    IQueryable<Pitanje> query = _context.Pitanja;

    if (oblastId.HasValue)
    {
        query = query.Where(p => p.Oblast != null && p.Oblast.Id == oblastId.Value);
    }

    var pitanja = await query
        .Select(p => new
        {
            p.Id,
            p.Tekst
        })
        .ToListAsync();

    if (pitanja == null || !pitanja.Any())
    {
        return NotFound($"Nije pronađeno pitanje{(oblastId.HasValue ? $" za specificiranu oblast (Oblast ID: {oblastId})" : "")}.");
    }

    return Ok(pitanja);
}


}