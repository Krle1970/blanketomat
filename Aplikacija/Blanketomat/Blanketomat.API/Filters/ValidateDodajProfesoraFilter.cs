using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters;

public class ValidateDodajProfesoraFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajProfesoraFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var profesor = context.ActionArguments["profesor"] as Profesor;
        if (profesor == null)
        {
            context.ModelState.AddModelError("Profesor", "Profesor objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else
        {
            if (_context.Profesori == null)
            {
                context.ModelState.AddModelError("Profesor", "Tabela Profesor ne postoji.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            else
            {
                var postojeciProfesor = _context.Profesori.FirstOrDefault(x =>
                    !string.IsNullOrWhiteSpace(profesor.Email) &&
                    !string.IsNullOrWhiteSpace(x.Email) &&
                    profesor.Email.ToLower() == x.Email.ToLower()
                    );

                if (postojeciProfesor != null)
                {
                    context.ModelState.AddModelError("Profesor", "Profesor vec postoji.");
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