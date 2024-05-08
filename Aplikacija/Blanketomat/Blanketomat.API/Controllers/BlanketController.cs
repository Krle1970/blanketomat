using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
 public class BlanketController : ControllerBase
 {
      private readonly BlanketomatContext _context;

      public BlanketController(BlanketomatContext context)
      {
        _context = context;
      }

    [HttpGet("{page}/{count}")]
    public async Task<ActionResult> VratiBlanket(int page,int count)
      {
        
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Blanketi.Count() / (float)brojRezultata);

        var blanketi = await _context.Blanketi
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Blanket>
        {
            Response = blanketi,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
      }

      [HttpGet("{id}")]
      [TypeFilter(typeof(ValidateIdFilter<Blanket>))]
      public async Task<ActionResult> VratiBlanket(int id)
      {
        return Ok(await _context.Blanketi.FindAsync(id));
      }

      [HttpDelete("{id}")]
      [TypeFilter(typeof(ValidateIdFilter<Blanket>))]
      public async Task<ActionResult> ObrisiBlanket(int id)
      {
         var blanketZaBrisanje = await _context.Blanketi.FindAsync(id);
         _context.Blanketi.Remove(blanketZaBrisanje!);
         await _context.SaveChangesAsync();

         return Ok(blanketZaBrisanje);
      }
      
      
      [HttpPost]
      public async Task<ActionResult> DodajBlanket([FromBody]Blanket blanket) 
      {
        await _context.Blanketi.AddAsync(blanket);
        await _context.SaveChangesAsync();

       return Ok(new { message = "Blanket je uspešno dodat.", blanket });
      }

      [HttpPut]
      public async Task<ActionResult> AzurirajBlanket([FromBody]Blanket blanket)
      {
        var BlanketZaPromenu = HttpContext.Items["blanket"] as Blanket;
        BlanketZaPromenu!.Tip=blanket.Tip;
        BlanketZaPromenu.Kategorija=blanket.Kategorija;
        BlanketZaPromenu.IspitniRok=blanket.IspitniRok;
        BlanketZaPromenu.Pitanja=blanket.Pitanja;
        BlanketZaPromenu.Zadaci=blanket.Zadaci;

        await _context.SaveChangesAsync();
        return NoContent();
      }


 }
