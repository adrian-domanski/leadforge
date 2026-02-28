using System.Security.Claims;
using LeadForge.Application;
using LeadForge.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadForge.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/generate/[controller]")]
public class GenerateController : ControllerBase
{
   private readonly IGenerationService _generationService;

   public GenerateController(IGenerationService generationService)
   {
      _generationService = generationService;
   }

   [HttpPost]
   public async Task<IActionResult> Generate(GeneratePostRequest request)
   {
      var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

      if (userIdClaim == null)
         return Unauthorized();

      var userId = Guid.Parse(userIdClaim);
      var result = await _generationService.GenerateAsync(
      userId, request.InputText, request.GoalType);

      return Ok(result);
   }
}