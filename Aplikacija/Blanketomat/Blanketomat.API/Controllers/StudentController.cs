using Blanketomat.API.Filters;
using Blanketomat.Domain.Models;
using Blanketomat.Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public StudentController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public ActionResult VratiSveStudente()
    {
        return Ok(_unitOfWork.StudentRepository.GetAll());
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(Student_ValidateStudentIdFilter<Student>))]
    public ActionResult VratiStudenta(int id)
    {
        return Ok(_unitOfWork.StudentRepository.Get(id));
    }

    [HttpPost]
    public ActionResult DodajStudenta([FromBody]Student student)
    {
        _unitOfWork.StudentRepository.Add(student);
        _unitOfWork.Save();

        return CreatedAtAction(nameof(VratiStudenta), new { id = student.Id }, student);
    }

    [HttpPut("{id}")]
    public ActionResult AzurirajStudenta(int id, [FromBody]Student student)
    {
        var studentZaUpdate = _unitOfWork.StudentRepository.Get(id);
        studentZaUpdate.Ime = student.Ime;
        studentZaUpdate.Prezime = student.Prezime;
        studentZaUpdate.Email = student.Email;
        studentZaUpdate.Password = student.Password;
        studentZaUpdate.Akreditacija = student.Akreditacija;
        studentZaUpdate.Smer = student.Smer;
        studentZaUpdate.Predmeti = student.Predmeti;

        _unitOfWork.Save();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult ObrisiStudenta(int id)
    {
        var studenZaBrisanje = _unitOfWork.StudentRepository.Get(id);
        _unitOfWork.StudentRepository.Remove(studenZaBrisanje);
        _unitOfWork.Save();

        return Ok(studenZaBrisanje);
    }
}