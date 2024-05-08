using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AsistentController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public AsistentController(BlanketomatContext context)
    {
        _context = context;
    }

   
    [HttpGet("{page}/{count}")]
    public async Task<ActionResult> VratiAsistenta(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Asistenti.Count() / (float)brojRezultata);

        var asistenti = await _context.Asistenti
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Asistent>
        {
            Response = asistenti,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };
        
        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Asistent>))]
    public async Task<ActionResult> VratiAsistenta(int id)
    {
        return Ok(await _context.Asistenti.FindAsync(id));
    }

    [HttpPost]
    //filter
    public async Task<ActionResult> DodajAsistenta([FromBody]Asistent asistent)
    {
        await _context.Asistenti.AddAsync(asistent);
        await _context.SaveChangesAsync();

       return Ok(new { message = "Asistent je uspešno dodat.", asistent });
    }

    [HttpPut]
    //filter<---
    public async Task<ActionResult> AzurirajAsistenta([FromBody]Asistent asistent)
    {
        var asistentZaPromenu= await _context.Asistenti.FindAsync(asistent.Id);
        //var ProfesorZaAzuriranje = HttpContext.Items["profesor"] as Profesor;
        asistentZaPromenu!.Ime=asistent.Ime;
        asistentZaPromenu.Prezime=asistent.Prezime;
        asistentZaPromenu.Email=asistent.Email;
        asistentZaPromenu.Password=asistent.Password;
        asistentZaPromenu.Smer=asistent.Smer;
        asistentZaPromenu.Predmeti=asistent.Predmeti;

        await _context.SaveChangesAsync();
    
        return NoContent();
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Asistent>))]
    public async Task<ActionResult> ObrisiAsistenta(int id)
    {
        var AsistentZaBrisanje = await _context.Predmeti.FindAsync(id);
        _context.Predmeti.Remove(AsistentZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok(AsistentZaBrisanje);
    }

}
