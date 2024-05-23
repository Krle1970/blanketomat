using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Helper;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.AdministratorFilters;

public class ValidateAzurirajAdministratoraFilter<T> : ActionFilterAttribute where T : class
{
    //private readonly BlanketomatContext _context;

    //public ValidateAzurirajAdministratoraFilter(BlanketomatContext context)
    //{
    //    _context = context;
    //}

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.ModelState.AddModelError($"{typeof(T).Name}", $"{typeof(T).Name}" + " objekat je nevalidan");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new NotFoundObjectResult(problemDetails);
        }

            //var administratorId = administrator.Id;
            //if (administratorId <= 0)
            //{
            //    context.ModelState.AddModelError("Administrator", "Nevalidan Id.");
            //    var problemDetails = new ValidationProblemDetails(context.ModelState)
            //    {
            //        Status = StatusCodes.Status400BadRequest
            //    };
            //    context.Result = new NotFoundObjectResult(problemDetails);
            //}
            //else
            //{
            //    if (_context.Administratori == null)
            //    {
            //        context.ModelState.AddModelError("Administrator", "Tabela Administratori ne postoji.");
            //        var problemDetails = new ValidationProblemDetails(context.ModelState)
            //        {
            //            Status = StatusCodes.Status404NotFound
            //        };
            //        context.Result = new NotFoundObjectResult(problemDetails);
            //    }
            //    else
            //    {
            //        var administratorZaAzuriranje = _context.Administratori.Find(administratorId);
            //        if (administratorZaAzuriranje == null)
            //        {
            //            context.ModelState.AddModelError("Administrator", "Administrator ne postoji u bazi podataka.");
            //            var problemDetails = new ValidationProblemDetails(context.ModelState)
            //            {
            //                Status = StatusCodes.Status404NotFound
            //            };
            //            context.Result = new NotFoundObjectResult(problemDetails);
            //        }
            //        else
            //        {
            //            context.HttpContext.Items["administrator"] = administratorZaAzuriranje;
            //        }
            //    }
            //}
    }
}