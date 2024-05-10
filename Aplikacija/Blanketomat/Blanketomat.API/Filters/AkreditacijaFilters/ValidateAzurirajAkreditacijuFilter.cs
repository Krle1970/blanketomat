using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.AkreditacijaFilters;

public class ValidateAzurirajAkreditacijuFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateAzurirajAkreditacijuFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var akreditacija = context.ActionArguments["akreditacija"] as Akreditacija;
        if (akreditacija == null)
        {
            context.ModelState.AddModelError("Akreditacija", "Akreditacija objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new NotFoundObjectResult(problemDetails);
        }
        else
        {
            var akreditacijaId = akreditacija.Id;
            if (akreditacijaId <= 0)
            {
                context.ModelState.AddModelError("Akreditacija", "Nevalidan Id.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            else
            {
                if (_context.Akreditacije == null)
                {
                    context.ModelState.AddModelError("Akreditacija", "Tabela Akreditacije ne postoji.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
                else
                {
                    var akreditacijaZaAzuriranje = _context.Akreditacije.Find(akreditacijaId);
                    if (akreditacijaZaAzuriranje == null)
                    {
                        context.ModelState.AddModelError("Akreditacija", "Akreditacija ne postoji u bazi podataka.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        context.HttpContext.Items["akreditacija"] = akreditacijaZaAzuriranje;
                    }
                }
            }
        }
    }
}