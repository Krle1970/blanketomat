using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.BlanketFilters;

public class ValidateDodajBlanketFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajBlanketFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var blanket = context.ActionArguments["blanket"] as Blanket;
        if (blanket == null)
        {
            context.ModelState.AddModelError("Blanket", "Blanket objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else
        {
            if (_context.Blanketi == null)
            {
                context.ModelState.AddModelError("Blanketi", "Tabela Blanketi ne postoji.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
        }
    }
}