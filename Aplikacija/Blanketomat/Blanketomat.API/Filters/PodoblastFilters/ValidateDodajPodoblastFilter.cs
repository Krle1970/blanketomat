using Blanketomat.API.Context;
using Blanketomat.API.DTOs.PodoblastDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.PodoblastFilters;

public class ValidateDodajPodoblastFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajPodoblastFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var podoblast = context.ActionArguments["novaPodoblast"] as DodajPodoblastDTO;
        var postojecaPodoblast = _context.Podoblasti.FirstOrDefault(x =>
            !string.IsNullOrWhiteSpace(podoblast!.Naziv) &&
            !string.IsNullOrWhiteSpace(x.Naziv) &&
            podoblast.Naziv.ToLower() == x.Naziv.ToLower()
            );

        if (postojecaPodoblast != null)
        {
            context.ModelState.AddModelError("Podoblast", "Podoblast vec postoji.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
    }
}