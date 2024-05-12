using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.SlikaFilters;

public class ValidateAzurirajSlikuFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateAzurirajSlikuFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var slika = context.ActionArguments["slika"] as Slika;
        if (slika == null)
        {
            context.ModelState.AddModelError("Slika", "Slika objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new NotFoundObjectResult(problemDetails);
        }
        else
        {
            var slikaId = slika.Id;
            if (slikaId <= 0)
            {
                context.ModelState.AddModelError("Slika", "Nevalidan Id.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            else
            {
                if (_context.Slike == null)
                {
                    context.ModelState.AddModelError("Slika", "Tabela Slike ne postoji.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
                else
                {
                    var slikaZaAzuriranje = _context.Slike.Find(slikaId);
                    if (slikaZaAzuriranje == null)
                    {
                        context.ModelState.AddModelError("Slika", "Slika ne postoji u bazi podataka.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        context.HttpContext.Items["slika"] = slikaZaAzuriranje;
                    }
                }
            }
        }
    }
}