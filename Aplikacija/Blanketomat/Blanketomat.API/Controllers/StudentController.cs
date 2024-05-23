using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.StudentFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public StudentController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("{page}/{count}")]
    public async Task<ActionResult> VratiStudente(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Studenti.Count() / (float)brojRezultata);

        var studenti = await _context.Studenti
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Student>
        {
            Podaci = studenti,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Student>))]
    public async Task<ActionResult> VratiStudenta(int id)
    {
        return Ok(await _context.Studenti.FindAsync(id));
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDodajStudentaFilter))]
    public async Task<ActionResult> DodajStudenta([FromBody]Student student)
    {
        _context.Studenti.Add(student);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiStudenta),
            new { id = student.Id },
            student
            );
    }

    [HttpPut]
    [TypeFilter(typeof(ValidateAzurirajStudentaFilter))]
    public async Task<ActionResult> AzurirajStudenta([FromBody]Student student)
    {
        var studentZaAzuriranje = HttpContext.Items["student"] as Student;
        studentZaAzuriranje!.Ime = student.Ime;
        studentZaAzuriranje.Prezime = student.Prezime;
        studentZaAzuriranje.Email = student.Email;
        //studentZaAzuriranje.Password = student.Password;
        studentZaAzuriranje.Akreditacija = student.Akreditacija;
        studentZaAzuriranje.Smer = student.Smer;
        studentZaAzuriranje.Predmeti = student.Predmeti;

        await _context.SaveChangesAsync();
        return Ok(studentZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Student>))]
    public async Task<ActionResult> ObrisiStudenta(int id)
    {
        var studentZaBrisanje = await _context.Studenti.FindAsync(id);
        _context.Studenti.Remove(studentZaBrisanje!);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}