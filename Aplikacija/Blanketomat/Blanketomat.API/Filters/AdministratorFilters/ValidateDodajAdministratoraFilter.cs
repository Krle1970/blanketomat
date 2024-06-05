using Blanketomat.API.Context;
using Blanketomat.API.DTOs.AdministratorDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.AdministratorFilters;

public class ValidateDodajAdministratoraFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajAdministratoraFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var administrator = context.ActionArguments["noviAdministrator"] as DodajAdministratoraDTO;
        if (administrator == null)
        {
            context.ModelState.AddModelError("Administrator", "Administrator objekat je null");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else
        {
            var postojeciAdministrator = _context.Administratori.FirstOrDefault(x =>
                !string.IsNullOrWhiteSpace(administrator.Email) &&
                !string.IsNullOrWhiteSpace(x.Email) &&
                administrator.Email.ToLower() == x.Email.ToLower()
                );

            if (postojeciAdministrator != null)
            {
                context.ModelState.AddModelError("Administrator", "Administrator sa ovom email adresom vec postoji");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}