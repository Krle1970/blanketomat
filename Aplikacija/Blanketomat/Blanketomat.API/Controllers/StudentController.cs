using Blanketomat.API.Context;
using Blanketomat.API.Filters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet]
    public ActionResult VratiStudente()
    {
        return Ok(_context.Studenti);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Student>))]
    public async Task<ActionResult> VratiStudenta(int id)
    {
        return Ok(await _context.Studenti.FindAsync(id));
    }

    [HttpPost("Dodaj")]
    [TypeFilter(typeof(ValidateDodajStudentaFilter))]
    public async Task<ActionResult> DodajStudenta([FromBody]Student student)
    {
        await _context.Studenti.AddAsync(student);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiStudenta),
            new { id = student.Id },
            student);
    }

    [HttpPut("Azuriraj")]
    [TypeFilter(typeof(ValidateAzurirajStudentaFilter))]
    public async Task<ActionResult> AzurirajStudenta([FromBody]Student student)
    {
        var studentZaAzuriranje = HttpContext.Items["student"] as Student;
        studentZaAzuriranje!.Ime = student.Ime;
        studentZaAzuriranje.Prezime = student.Prezime;
        studentZaAzuriranje.Email = student.Email;
        studentZaAzuriranje.Password = student.Password;
        studentZaAzuriranje.Akreditacija = student.Akreditacija;
        studentZaAzuriranje.Smer = student.Smer;
        studentZaAzuriranje.Predmeti = student.Predmeti;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("Obrisi/{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Student>))]
    public async Task<ActionResult> ObrisiStudenta(int id)
    {
        var studentZaBrisanje = await _context.Studenti.FindAsync(id);
        _context.Studenti.Remove(studentZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok(studentZaBrisanje);
    }
}