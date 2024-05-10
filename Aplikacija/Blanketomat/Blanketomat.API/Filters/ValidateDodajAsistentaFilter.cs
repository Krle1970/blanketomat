using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters;

public class ValidateDodajAsistentaFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajAsistentaFilter(BlanketomatContext context)
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
            context.Result = new BadRequestObjectResult(problemDetails);
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
                var postojeciAsistent = _context.Asistenti.FirstOrDefault(x =>
                    !string.IsNullOrWhiteSpace(asistent.Email) &&
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
}