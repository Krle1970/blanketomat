using Blanketomat.Domain.Models;
using Blanketomat.Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PredmetController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public PredmetController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public ActionResult VratiSvePredmete()
    {
        return Ok(_unitOfWork.PredmetRepository.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult VratiPredmet(int id)
    {
        return Ok(_unitOfWork.PredmetRepository.Get(id));
    }

    [HttpPost]
    public ActionResult DodajPredmet([FromBody]Predmet predmet)
    {
        _unitOfWork.PredmetRepository.Add(predmet);
        _unitOfWork.Save();

        return CreatedAtAction(nameof(VratiPredmet), new { id = predmet.Id }, predmet);
    }

    [HttpPut("{id}")]
    public ActionResult AzurirajPredmet(int id, [FromBody]Predmet predmet)
    {
        var predmetZaAzuriranje = _unitOfWork.PredmetRepository.Get(id);
        predmetZaAzuriranje.Naziv = predmet.Naziv;
        predmetZaAzuriranje.Godina = predmet.Godina;
        predmetZaAzuriranje.Akreditacija = predmet.Akreditacija;
        predmetZaAzuriranje.Smer = predmet.Smer;
        predmetZaAzuriranje.Profesori = predmet.Profesori;
        predmetZaAzuriranje.Asistenti = predmet.Asistenti;
        predmetZaAzuriranje.Studenti = predmet.Studenti;

        _unitOfWork.Save();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult ObrisiPredmet(int id)
    {
        var predmetZaBrisanje = _unitOfWork.PredmetRepository.Get(id);
        _unitOfWork.PredmetRepository.Remove(predmetZaBrisanje);

        _unitOfWork.Save();
        return Ok(predmetZaBrisanje);
    }
}