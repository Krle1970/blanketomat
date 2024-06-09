using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.DTOs.BlanketDTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class BlanketController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public BlanketController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("blanketi")]
    [TypeFilter(typeof(ValidateDbSetFilter<Blanket>))]
    public ActionResult<IEnumerable<Blanket>> VratiSveBlankete()
    {
        return _context.Blanketi;
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Blanket>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult> VratiBlankete(int page, int count)
    {
        
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Blanketi.Count() / (float)brojRezultata);

        var blanketi = await _context.Blanketi
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PagingResponseDTO<Blanket>
        {
            Podaci = blanketi,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Blanket>))]
    [TypeFilter(typeof(ValidateIdFilter<Blanket>))]
    public ActionResult<Blanket> VratiBlanket(int id)
    {
        return Ok(HttpContext.Items["entity"] as Blanket);
    }
      
    [HttpPost]
    [TypeFilter(typeof(ValidateDbSetFilter<Blanket>))]
    //[ValidateDodajBlanketFilter]
    public async Task<ActionResult> DodajBlanket([FromBody]DodajBlanketDTO noviBlanket) 
    {
        Blanket blanket = new Blanket
        {
            Tip = noviBlanket.Tip,
            Kategorija = noviBlanket.Kategorija,
            Putanja = noviBlanket.Putanja,
            Slike = noviBlanket.Slike,
            Predmet = noviBlanket.Predmet,
            IspitniRok = noviBlanket.PonavljanjeIspitnogRoka,
            Pitanja = noviBlanket.Pitanja,
            Zadaci = noviBlanket.Zadaci
        };

        _context.Blanketi.Add(blanket);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiBlanket),
            new { id = blanket.Id },
            blanket
            );
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Blanket>))]
    [TypeFilter(typeof(ValidateIdFilter<Blanket>))]
    public async Task<ActionResult> AzurirajBlanket(int id, [FromBody]AzurirajBlanketDTO blanket)
    {
        // iz ValidateIdFilter-a
        var blanketZaAzuriranje = HttpContext.Items["entity"] as Blanket;

        blanketZaAzuriranje!.Tip = blanket.Tip;
        blanketZaAzuriranje.Kategorija = blanket.Kategorija;
        blanketZaAzuriranje.Putanja = blanket.Putanja;
        blanketZaAzuriranje.Slike = blanket.Slike;
        blanketZaAzuriranje.Predmet = blanket.Predmet;
        blanketZaAzuriranje.IspitniRok = blanket.PonavljanjeIspitnogRoka;
        blanketZaAzuriranje.Pitanja = blanket.Pitanja;
        blanketZaAzuriranje.Zadaci = blanket.Zadaci;

        await _context.SaveChangesAsync();
        return Ok(blanketZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Blanket>))]
    [TypeFilter(typeof(ValidateIdFilter<Blanket>))]
    public async Task<ActionResult<string>> ObrisiBlanket(int id)
    {
        // iz ValidateIdFilter-a
        var blanketZaBrisanje = HttpContext.Items["entity"] as Blanket;
        _context.Blanketi.Remove(blanketZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Blanket uspe�no izbrisan");
    }
    
    [HttpGet("{blanketId}/predmet")]
        public ActionResult<IEnumerable<Podoblast>> GetPredmetForBlanket(int blanketId)
        {
            var blanket = _context.Blanketi
                .Include(o => o.Predmet)
                .FirstOrDefault(o => o.Id == blanketId);

            if (blanket == null)
            {
                return NotFound("Blanket nije pronađen.");
            }

            var predmet = blanket.Predmet;

            if (predmet == null)
            {
                return Ok(new List<Predmet>());
            }

            return Ok(predmet);
        }
    [HttpPost("{blanketId}/Predmet")]
        public ActionResult<Predmet> CreatePredmetForBlanket(int blanketId, [FromBody] Predmet newPredmet)
        {
            var blanket = _context.Blanketi.FirstOrDefault(o => o.Id == blanketId);

            if (blanket == null)
            {
                return NotFound("Blanket nije pronađen.");
            }
            blanket.Predmet = newPredmet;
            _context.Predmeti.Add(newPredmet);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetPredmetForBlanket), new { blanketId = blanketId }, newPredmet);
        }
        
}