using Blanketomat.Domain.Models;
using Blanketomat.Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AsistentController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public AsistentController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public ActionResult VratiSveAsistente()
    {
        return Ok(_unitOfWork.AsistentRepository.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult VratiAsistenta(int id)
    {
        return Ok(_unitOfWork.AsistentRepository.Get(id));
    }

    [HttpPost]
    public ActionResult DodajAsistenta([FromBody] Asistent asistent)
    {
        _unitOfWork.AsistentRepository.Add(asistent);
        _unitOfWork.Save();

        return CreatedAtAction(nameof(VratiAsistenta), new { id = asistent.Id }, asistent);
    }

    [HttpPut("{id}")]
    public ActionResult AzurirajAsistenta(int id, [FromBody] Asistent asistent)
    {
        var asistentZaAzuriranje = _unitOfWork.ProfesorRepository.Get(id);
        asistentZaAzuriranje.Ime = asistent.Ime;
        asistentZaAzuriranje.Prezime = asistent.Prezime;
        asistentZaAzuriranje.Email = asistent.Email;
        asistentZaAzuriranje.Password = asistent.Password;
        asistentZaAzuriranje.Smer = asistent.Smer;
        asistentZaAzuriranje.Predmeti = asistent.Predmeti;

        _unitOfWork.Save();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult ObrisiAsistenta(int id)
    {
        var asistentZaBrisanje = _unitOfWork.AsistentRepository.Get(id);
        _unitOfWork.AsistentRepository.Remove(asistentZaBrisanje);

        _unitOfWork.Save();
        return Ok(asistentZaBrisanje);
    }
}