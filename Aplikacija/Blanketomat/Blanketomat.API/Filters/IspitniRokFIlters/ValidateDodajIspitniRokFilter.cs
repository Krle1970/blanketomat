using Blanketomat.API.Context;
using Blanketomat.API.DTOs.IspitniRokDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.IspitniRokFIlters;

public class ValidateDodajIspitniRokFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajIspitniRokFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var ispitniRok = context.ActionArguments["noviIspitniRok"] as DodajIspitniRokDTO;
        if (!context.ModelState.IsValid)
        {
            context.ModelState.AddModelError("IspitniRok", "Ispitni rok objekat je navalidan");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else
        {
            var postojeciIspitniRok = _context.IspitniRokovi.FirstOrDefault(x =>
                !string.IsNullOrWhiteSpace(ispitniRok!.Naziv) &&
                !string.IsNullOrWhiteSpace(x.Naziv) &&
                ispitniRok.Naziv.ToLower() == x.Naziv.ToLower()
                );

            if (postojeciIspitniRok != null)
            {
                context.ModelState.AddModelError("IspitniRok", "Ispitni rok vec postoji.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}