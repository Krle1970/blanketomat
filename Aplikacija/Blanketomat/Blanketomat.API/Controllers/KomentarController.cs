using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KomentarController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public KomentarController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("{page}/{count}")]
    public async Task<ActionResult> VratiKomentare(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Komentari.Count() / (float)brojRezultata);

        var komentari = await _context.Komentari
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Komentar>
        {
            Response = komentari,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Komentar>))]
    public async Task<ActionResult> VratiKomentar(int id)
    {
        return Ok(await _context.Komentari.FindAsync(id));
    }

    [HttpPost]
        public async Task<ActionResult> DodajKomentar([FromBody]Komentar komentar)
    {
        await _context.Komentari.AddAsync(komentar);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiKomentar),
            new { id = komentar.Id },
            komentar);
    }

    [HttpPut]
    
    public async Task<ActionResult> AzurirajKomentar([FromBody]Komentar komentar)
    {
        var komentarZaAzuriranje = HttpContext.Items["komentar"] as Komentar;
        
        komentarZaAzuriranje!.Tekst=komentar.Tekst;
        komentarZaAzuriranje.Slike=komentar.Slike;
        komentarZaAzuriranje.Lajkovi=komentar.Lajkovi;
        komentarZaAzuriranje.StudentPostavio=komentar.StudentPostavio;
        komentarZaAzuriranje.ProfesoriLiked=komentar.ProfesoriLiked;
        komentarZaAzuriranje.AsistentiLiked=komentar.AsistentiLiked;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Komentar>))]
    public async Task<ActionResult> ObrisiKomentar(int id)
    {
        var KomentarZaBrisanje = await _context.Komentari.FindAsync(id);
        _context.Komentari.Remove(KomentarZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok(KomentarZaBrisanje);
    }
}