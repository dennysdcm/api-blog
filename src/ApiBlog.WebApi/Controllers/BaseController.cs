using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace ApiBlog.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public abstract class BaseController : ControllerBase
{
    protected ActionResult<TResult> OkOrNotFound<TResult>(TResult result)
        => result is null ? NotFound() : Ok(result);
}