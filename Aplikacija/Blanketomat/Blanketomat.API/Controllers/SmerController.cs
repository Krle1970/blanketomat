using Blanketomat.Domain.Models;
using Blanketomat.Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SmerController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public SmerController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public ActionResult VratiSveSmerove()
    {
        return Ok(_unitOfWork.SmerRepository.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult VratiSmer(int id)
    {
        return Ok(_unitOfWork.SmerRepository.Get(id));
    }

    [HttpPost]
    public ActionResult DodajSmer([FromBody]Smer smer)
    {
        _unitOfWork.SmerRepository.Add(smer);
        _unitOfWork.Save();

        return CreatedAtAction(nameof(Smer), new { id = smer.Id }, smer);
    }

    [HttpPut("{id}")]
    public ActionResult AzurirajSmer(int id, [FromBody]Smer smer)
    {
        var smerZaAzuriranje = _unitOfWork.SmerRepository.Get(smer.Id);
        smerZaAzuriranje.Naziv = smer.Naziv;
        smerZaAzuriranje.Predmeti = smer.Predmeti;
        smerZaAzuriranje.Profesori = smer.Profesori;
        smerZaAzuriranje.Asistenti = smer.Asistenti;
        smerZaAzuriranje.Studenti = smer.Studenti;

        _unitOfWork.Save();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult ObrisiSmer(int id)
    {
        var smerZaBrisanje = _unitOfWork.SmerRepository.Get(id);
        _unitOfWork.SmerRepository.Remove(smerZaBrisanje);
        _unitOfWork.Save();

        return Ok(smerZaBrisanje);
    }
}