using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.KomentarFilters;

public class ValidateAzurirajKomentarFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateAzurirajKomentarFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var komentar = context.ActionArguments["komentar"] as Komentar;
        if (komentar == null)
        {
            context.ModelState.AddModelError("Komentar", "Komentar objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new NotFoundObjectResult(problemDetails);
        }
        else
        {
            var komentarId = komentar.Id;
            if (komentarId <= 0)
            {
                context.ModelState.AddModelError("Komentar", "Nevalidan Id.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            else
            {
                if (_context.Komentari == null)
                {
                    context.ModelState.AddModelError("Komentar", "Tabela Komentari ne postoji.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
                else
                {
                    var komentarZaAzuriranje = _context.Komentari.Find(komentarId);
                    if (komentarZaAzuriranje == null)
                    {
                        context.ModelState.AddModelError("Komentar", "Komentar ne postoji u bazi podataka.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        context.HttpContext.Items["komentar"] = komentarZaAzuriranje;
                    }
                }
            }
        }
    }
}