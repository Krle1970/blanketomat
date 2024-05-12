using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters.AuthenticationFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public AuthenticationController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateAuthenticationLoginFilter))]
    public ActionResult Login([FromBody] LoginDTO? user)
    {
        string fullName = user!.FullName!;
        return Ok(fullName);
    }
}