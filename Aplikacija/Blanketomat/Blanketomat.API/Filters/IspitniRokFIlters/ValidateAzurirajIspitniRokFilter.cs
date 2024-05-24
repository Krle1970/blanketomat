using Blanketomat.API.Context;
using Blanketomat.API.DTOs.IspitniRokDTOs;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.IspitniRokFIlters;

public class ValidateAzurirajIspitniRokFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateAzurirajIspitniRokFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var ispitniRok = context.ActionArguments["ispitniRok"] as AzurirajIspitniRokDTO;
        if (ispitniRok == null)
        {
            context.ModelState.AddModelError("IspitniRok", "IspitniRok objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new NotFoundObjectResult(problemDetails);
        }
    }
}