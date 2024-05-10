using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters;

public class ValidateDodajOblastFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajOblastFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var oblast = context.ActionArguments["oblast"] as Oblast;
        if (oblast == null)
        {
            context.ModelState.AddModelError("Oblast", "Oblast objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else
        {
            if (_context.Oblasti == null)
            {
                context.ModelState.AddModelError("Oblast", "Tabela Oblasti ne postoji.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            else
            {
                var postojecaOblast = _context.Oblasti.FirstOrDefault(x =>
                    !string.IsNullOrWhiteSpace(oblast.Naziv) &&
                    !string.IsNullOrWhiteSpace(x.Naziv) &&
                    oblast.Naziv.ToLower() == x.Naziv.ToLower()
                    );

                if (postojecaOblast != null)
                {
                    context.ModelState.AddModelError("Oblast", "Oblast vec postoji.");
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