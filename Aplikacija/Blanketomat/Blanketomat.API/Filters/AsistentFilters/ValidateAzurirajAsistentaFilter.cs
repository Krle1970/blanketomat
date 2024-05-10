using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.AsistentFilters;

public class ValidateAzurirajAsistentaFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateAzurirajAsistentaFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var asistent = context.ActionArguments["asistent"] as Asistent;
        if (asistent == null)
        {
            context.ModelState.AddModelError("Asistent", "Asistent objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new NotFoundObjectResult(problemDetails);
        }
        else
        {
            var asistentId = asistent.Id;
            if (asistentId <= 0)
            {
                context.ModelState.AddModelError("Asistent", "Nevalidan Id.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            else
            {
                if (_context.Asistenti == null)
                {
                    context.ModelState.AddModelError("Asistent", "Tabela Asistenti ne postoji.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
                else
                {
                    var asistentZaAzuriranje = _context.Asistenti.Find(asistentId);
                    if (asistentZaAzuriranje == null)
                    {
                        context.ModelState.AddModelError("Asistent", "Asistent ne postoji u bazi podataka.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        context.HttpContext.Items["asistent"] = asistentZaAzuriranje;
                    }
                }
            }
        }
    }
}