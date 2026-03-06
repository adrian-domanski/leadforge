using LeadForge.Application;
using LeadForge.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadForge.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/generate/[controller]")]
public class GenerationController : ControllerBase
{
   private readonly IGenerationService _generationService;

   public GenerationController(IGenerationService generationService)
   {
      _generationService = generationService;
   }

   [HttpPost]
   public async Task<ActionResult> CreateGeneration(
      GeneratePostRequest request,
      CancellationToken ct = default )
   {
      var result = await _generationService.GenerateAsync(request,
         ct);

      return Ok(result);
   }

   [HttpGet]
   public async Task<ActionResult<PagedResult<GenerationListItem>>> GetUserGenerations(
      [FromQuery] int page = 1,
      [FromQuery] int pageSize = 10,
      CancellationToken ct = default)
   {
      var result = await _generationService.GetUserGenerationsAsync(
         page,
         pageSize,
         ct);

      return Ok(result);
   }

   [HttpGet("{id:guid}")]
   public async Task<ActionResult<GenerationListItem>> GetById(
      Guid id,
      CancellationToken ct = default)
   {
      var result = await _generationService.GetByIdAsync(
         id,
         ct);

      return Ok(result);
   }
}