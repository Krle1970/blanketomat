using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.ZadatakFilters;

public class ValidateAzurirajZadatakFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateAzurirajZadatakFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var zadatak = context.ActionArguments["zadatak"] as Zadatak;
        if (zadatak == null)
        {
            context.ModelState.AddModelError("Zadatak", "Zadatak objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new NotFoundObjectResult(problemDetails);
        }
        else
        {
            var zadatakId = zadatak.Id;
            if (zadatakId <= 0)
            {
                context.ModelState.AddModelError("Zadatak", "Nevalidan Id.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            else
            {
                if (_context.Zadaci == null)
                {
                    context.ModelState.AddModelError("Zadatak", "Tabela Zadaci ne postoji.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
                else
                {
                    var zadatakZaAzuriranje = _context.Zadaci.Find(zadatakId);
                    if (zadatakZaAzuriranje == null)
                    {
                        context.ModelState.AddModelError("Zadatak", "Zadatak ne postoji u bazi podataka.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        context.HttpContext.Items["zadatak"] = zadatakZaAzuriranje;
                    }
                }
            }
        }
    }
}