using Blanketomat.API.Context;
using Blanketomat.API.DTOs.KatedraDTOs;
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
        if (!context.ModelState.IsValid)
        {
            context.ModelState.AddModelError("Katedra", "Katedra objekat nije validan");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else
        {
            var katedra = context.ActionArguments["novaKatedra"] as DodajKatedruDTO;
            var postojecaKatedra = _context.Katedre.FirstOrDefault(x =>
                !string.IsNullOrWhiteSpace(katedra!.Naziv) &&
                !string.IsNullOrWhiteSpace(x.Naziv) &&
                katedra.Naziv.ToLower() == x.Naziv.ToLower()
                );

            if (postojecaKatedra != null)
            {
                context.ModelState.AddModelError("Katedra", "Katedra vec postoji");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}