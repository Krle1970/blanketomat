using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.KatedraFilters;

public class ValidateDodajKatedruFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajKatedruFilter(BlanketomatContext context)
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
            context.Result = new BadRequestObjectResult(problemDetails);
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
                var postojecaKatedra = _context.Katedre.FirstOrDefault(x =>
                    !string.IsNullOrWhiteSpace(katedra.Naziv) &&
                    !string.IsNullOrWhiteSpace(x.Naziv) &&
                    katedra.Naziv.ToLower() == x.Naziv.ToLower()
                    );

                if (postojecaKatedra != null)
                {
                    context.ModelState.AddModelError("Katedra", "Katedra vec postoji.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
            }
        }
    }
}