using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.KatedraFilters;

public class ValidateAzurirajKatedruFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateAzurirajKatedruFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var katedra = context.ActionArguments["katedra"] as Katedra;
        if (katedra == null)
        {
            context.ModelState.AddModelError("Katedra", "Katedra objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new NotFoundObjectResult(problemDetails);
        }
        else
        {
            var katedraId = katedra.Id;
            if (katedraId <= 0)
            {
                context.ModelState.AddModelError("Katedra", "Nevalidan Id.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            else
            {
                if (_context.Katedre == null)
                {
                    context.ModelState.AddModelError("Katedra", "Tabela Katedre ne postoji.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
                else
                {
                    var katedraZaAzuriranje = _context.Katedre.Find(katedraId);
                    if (katedraZaAzuriranje == null)
                    {
                        context.ModelState.AddModelError("Katedra", "Katedra ne postoji u bazi podataka.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        context.HttpContext.Items["katedra"] = katedraZaAzuriranje;
                    }
                }
            }
        }
    }
}