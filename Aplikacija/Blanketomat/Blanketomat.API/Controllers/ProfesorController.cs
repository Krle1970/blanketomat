using Blanketomat.Domain.Models;
using Blanketomat.Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfesorController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ProfesorController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public ActionResult VratiSveProfesore()
    {
        return Ok(_unitOfWork.ProfesorRepository.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult VratiProfesora(int id)
    {
        return Ok(_unitOfWork.ProfesorRepository.Get(id));
    }

    [HttpPost]
    public ActionResult DodajProfesora([FromBody]Profesor profesor)
    {
        _unitOfWork.ProfesorRepository.Add(profesor);
        _unitOfWork.Save();

        return CreatedAtAction(nameof(VratiProfesora), new { id = profesor.Id }, profesor);
    }

    [HttpPut("{id}")]
    public ActionResult AzurirajProfesora(int id, [FromBody]Profesor profesor)
    {
        var profesorZaAzuriranje = _unitOfWork.ProfesorRepository.Get(id);
        profesorZaAzuriranje.Ime = profesor.Ime;
        profesorZaAzuriranje.Prezime = profesor.Prezime;
        profesorZaAzuriranje.Email = profesor.Email;
        profesorZaAzuriranje.Password = profesor.Password;
        profesorZaAzuriranje.Smer = profesor.Smer;
        profesorZaAzuriranje.Predmeti = profesor.Predmeti;

        _unitOfWork.Save();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult ObrisiProfesora(int id)
    {
        var profesorZaBrisanje = _unitOfWork.ProfesorRepository.Get(id);
        _unitOfWork.ProfesorRepository.Remove(profesorZaBrisanje);

        _unitOfWork.Save();
        return Ok(profesorZaBrisanje);
    }
}