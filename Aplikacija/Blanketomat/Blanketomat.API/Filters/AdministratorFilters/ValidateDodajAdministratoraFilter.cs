using Blanketomat.API.Context;
using Blanketomat.API.Models;
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

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var administrator = context.ActionArguments["administrator"] as Administrator;
        if (administrator == null)
        {
            context.ModelState.AddModelError("Administrator", "Administrator objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else
        {
            if (_context.Administratori == null)
            {
                context.ModelState.AddModelError("Administrator", "Tabela Administratori ne postoji.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetails);
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
                    context.ModelState.AddModelError("Administrator", "Administrator vec postoji.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
            }
        }

        await next();
    }
}