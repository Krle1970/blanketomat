using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.GenericFilters;

public class ValidatePaginationFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var pageNumber = context.ActionArguments["page"] as int?;
        var itemsPerPage = context.ActionArguments["count"] as int?;

        if (pageNumber != null && itemsPerPage != null)
        {
            if (pageNumber <= 0 || itemsPerPage <= 0)
            {
                context.ModelState.AddModelError("Pagination", "Broj stranice i broj podataka po stranici moraju da budu pozitivni brojevi");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}