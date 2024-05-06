using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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


#pragma warning disable ASP0023 // Route conflict detected between controller actions
    [HttpGet("{page}/{count}")]
#pragma warning restore ASP0023 // Route conflict detected between controller actions
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

#pragma warning disable ASP0023 
    [HttpGet("{tip}/{kategorija}")]//mislio sam mozda da se doda neki tip godina u modelu da se mozda na taj nacin pretrazuju blanketi, ili cemo to preko PonavljanjeRoka
#pragma warning restore ASP0023 // Route conflict detected between controller actions
    public async Task<ActionResult> VratiBlanket(string tip, string kategorija)
      {
         var blanket=await _context.Blanketi.FirstOrDefaultAsync(x=> x.Tip==tip && x.Kategorija==kategorija);
         if(blanket==null)
         {
           return NotFound();
         }
         return Ok(blanket);
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
