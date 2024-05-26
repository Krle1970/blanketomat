using Blanketomat.API.Context;
using Blanketomat.API.DTOs.AsistentDTOs;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.AsistentFilters;

public class ValidateDodajAsistentaFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajAsistentaFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.ModelState.AddModelError("Asistent", "Asistent objekat nije validan");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else
        {
            var asistent = context.ActionArguments["noviAsistent"] as AsistentDTO;
            var postojeciAsistent = _context.Asistenti.FirstOrDefault(x =>
                !string.IsNullOrWhiteSpace(asistent!.Email) &&
                !string.IsNullOrWhiteSpace(x.Email) &&
                asistent.Email.ToLower() == x.Email.ToLower()
                );

            if (postojeciAsistent != null)
            {
                context.ModelState.AddModelError("Asistent", "Asistent vec postoji.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}