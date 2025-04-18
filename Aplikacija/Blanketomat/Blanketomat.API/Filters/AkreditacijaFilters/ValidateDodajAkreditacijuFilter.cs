﻿using Blanketomat.API.Context;
using Blanketomat.API.DTOs.AkreditacijaDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.AkreditacijaFilters;

public class ValidateDodajAkreditacijuFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajAkreditacijuFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var akreditacija = context.ActionArguments["novaAkreditacija"] as DodajAkreditacijuDTO;
        var postojecaAkreditacija = _context.Akreditacije.FirstOrDefault(x =>
            !string.IsNullOrWhiteSpace(akreditacija!.Naziv) &&
            !string.IsNullOrWhiteSpace(x.Naziv) &&
            akreditacija.Naziv.ToLower() == x.Naziv.ToLower()
            );

        if (postojecaAkreditacija != null)
        {
            context.ModelState.AddModelError("Akreditacija", "Akreditacija vec postoji.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
    }
}