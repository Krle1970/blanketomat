using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters;

public class ValidateAzurirajPitanjeFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateAzurirajPitanjeFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var pitanje = context.ActionArguments["pitanje"] as Pitanje;
        if (pitanje == null)
        {
            context.ModelState.AddModelError("Pitanje", "Pitanje objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new NotFoundObjectResult(problemDetails);
        }
        else
        {
            var pitanjeId = pitanje.Id;
            if (pitanjeId <= 0)
            {
                context.ModelState.AddModelError("Pitanje", "Nevalidan Id.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            else
            {
                if (_context.Pitanja == null)
                {
                    context.ModelState.AddModelError("Pitanje", "Tabela Pitanja ne postoji.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
                else
                {
                    var pitanjeZaAzuriranje = _context.Pitanja.Find(pitanjeId);
                    if (pitanjeZaAzuriranje == null)
                    {
                        context.ModelState.AddModelError("Pitanje", "Pitanje ne postoji u bazi podataka.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        context.HttpContext.Items["pitanje"] = pitanjeZaAzuriranje;
                    }
                }
            }
        }
    }
}