using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadForge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [Authorize]
    [HttpGet]
    public IActionResult Protected()
    {
        return Ok("You are authorized.");
    }
}