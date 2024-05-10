using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters;

public class ValidateAzurirajPodoblastFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateAzurirajPodoblastFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var podoblast = context.ActionArguments["podoblast"] as Podoblast;
        if (podoblast == null)
        {
            context.ModelState.AddModelError("Podoblast", "Podoblast objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new NotFoundObjectResult(problemDetails);
        }
        else
        {
            var podoblastId = podoblast.Id;
            if (podoblastId <= 0)
            {
                context.ModelState.AddModelError("Podoblast", "Nevalidan Id.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            else
            {
                if (_context.Podoblasti == null)
                {
                    context.ModelState.AddModelError("Podoblast", "Tabela Podoblasti ne postoji.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
                else
                {
                    var podoblastZaAzuriranje = _context.Blanketi.Find(podoblastId);
                    if (podoblastZaAzuriranje == null)
                    {
                        context.ModelState.AddModelError("Podoblast", "Podoblast ne postoji u bazi podataka.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        context.HttpContext.Items["podoblast"] = podoblastZaAzuriranje;
                    }
                }
            }
        }
    }
}