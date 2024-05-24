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

        var response = new PaginationResponseDTO<Blanket>
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
            Putanja = noviBlanket.Putanja
        };

        if (noviBlanket.SlikeIds != null)
        {
            Slika? slika;
            for (int i = 0; i < noviBlanket.SlikeIds.Count(); i++)
            {
                slika = await _context.Slike.FindAsync(noviBlanket.SlikeIds[i]);
                if (slika != null)
                {
                    if (!blanket.Slike!.Contains(slika))
                        blanket.Slike.Add(slika);
                }
            }
        }

        if (noviBlanket.PredmetId != null)
        {
            Predmet? predmet = await _context.Predmeti.FindAsync(noviBlanket.PredmetId);
            if (predmet != null)
            {
                blanket.Predmet = predmet;
            }
        }

        if (noviBlanket.PonavljanjeIspitnogRokaId != null)
        {
            PonavljanjeIspitnogRoka? ponavljanjeIspitnogRoka = await _context.PonavljanjaIspitnihRokova.FindAsync(noviBlanket.PonavljanjeIspitnogRokaId);
            if (ponavljanjeIspitnogRoka != null)
            {
                blanket.IspitniRok = ponavljanjeIspitnogRoka;
            }
        }

        if (noviBlanket.PitanjaIds != null)
        {
            Pitanje? pitanje;
            for (int i = 0; i < noviBlanket.PitanjaIds.Count(); i++)
            {
                pitanje = await _context.Pitanja.FindAsync(noviBlanket.PitanjaIds[i]);
                if (pitanje != null)
                {
                    if (!blanket.Pitanja!.Contains(pitanje))
                        blanket.Pitanja.Add(pitanje);
                }
            }
        }

        if (noviBlanket.ZadaciIds != null)
        {
            Zadatak? zadatak;
            for (int i = 0; i < noviBlanket.ZadaciIds.Count(); i++)
            {
                zadatak = await _context.Zadaci.FindAsync(noviBlanket.ZadaciIds[i]);
                if (zadatak != null)
                {
                    if (!blanket.Zadaci!.Contains(zadatak))
                        blanket.Zadaci.Add(zadatak);
                }
            }
        }

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

        if (blanket.SlikeIds != null)
        {
            Slika? slika;
            for (int i = 0; i < blanket.SlikeIds.Count(); i++)
            {
                slika = await _context.Slike.FindAsync(blanket.SlikeIds[i]);
                if (slika != null)
                {
                    if (!blanketZaAzuriranje.Slike!.Contains(slika))
                        blanketZaAzuriranje.Slike.Add(slika);
                }
            }
        }

        if (blanket.PredmetId != null)
        {
            Predmet? predmet = await _context.Predmeti.FindAsync(blanket.PredmetId);
            if (predmet != null)
            {
                blanketZaAzuriranje.Predmet = predmet;
            }
        }

        if (blanket.PonavljanjeIspitnogRokaId != null)
        {
            PonavljanjeIspitnogRoka? ponavljanjeIspitnogRoka = await _context.PonavljanjaIspitnihRokova.FindAsync(blanket.PonavljanjeIspitnogRokaId);
            if (ponavljanjeIspitnogRoka != null)
            {
                blanketZaAzuriranje.IspitniRok = ponavljanjeIspitnogRoka;
            }
        }

        if (blanket.PitanjaIds != null)
        {
            Pitanje? pitanje;
            for (int i = 0; i < blanket.PitanjaIds.Count(); i++)
            {
                pitanje = await _context.Pitanja.FindAsync(blanket.PitanjaIds[i]);
                if (pitanje != null)
                {
                    if (!blanketZaAzuriranje.Pitanja!.Contains(pitanje))
                        blanketZaAzuriranje.Pitanja.Add(pitanje);
                }
            }
        }

        if (blanket.ZadaciIds != null)
        {
            Zadatak? zadatak;
            for (int i = 0; i < blanket.ZadaciIds.Count(); i++)
            {
                zadatak = await _context.Zadaci.FindAsync(blanket.ZadaciIds[i]);
                if (zadatak != null)
                {
                    if (!blanketZaAzuriranje.Zadaci!.Contains(zadatak))
                        blanketZaAzuriranje.Zadaci.Add(zadatak);
                }
            }
        }

        if (blanket.KomentariIds != null)
        {
            Komentar? komentar;
            for (int i = 0; i < blanket.KomentariIds.Count(); i++)
            {
                komentar = await _context.Komentari.FindAsync(blanket.KomentariIds[i]);
                if (komentar != null)
                {
                    if (!blanketZaAzuriranje.Komentari!.Contains(komentar))
                        blanketZaAzuriranje.Komentari.Add(komentar);
                }
            }
        }

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

        return Ok("Blanket uspešno izbrisan");
    }
}