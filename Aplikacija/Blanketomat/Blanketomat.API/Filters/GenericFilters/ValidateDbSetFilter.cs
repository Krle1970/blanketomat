using Blanketomat.API.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.GenericFilters;

public class ValidateDbSetFilter<T> : ActionFilterAttribute where T : class
{
    private readonly BlanketomatContext _context;

    public ValidateDbSetFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (_context == null || _context.Set<T>() == null)
        {
            context.ModelState.AddModelError($"{typeof(T).Name}", $"Tabela" + $" {typeof(T).Name}s" + "ne postoji u bazi podataka.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status404NotFound
            };
            context.Result = new NotFoundObjectResult(problemDetails);
        }
    }
}