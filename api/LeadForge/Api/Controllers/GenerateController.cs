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

      var result = await _generationService.GenerateAsync(request);

      return Ok(result);
   }

   [HttpGet]
   public async Task<IActionResult> GetUserGenerations(GeneratePostRequest request)
   {

      var result = await _generationService.GenerateAsync(request);

      return Ok(result);
   }
}