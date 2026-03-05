using LeadForge.Application.Interfaces;
using LeadForge.Domain;
using LeadForge.Domain.Exceptions;
using LeadForge.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LeadForge.Application;

public class GenerationService : IGenerationService
{
    private readonly AppDbContext _db;
    private readonly IOpenAiService _openAiService;

    public GenerationService(AppDbContext db, IOpenAiService openAiService)
    {
        _db = db;
        _openAiService = openAiService;
    }

    public async Task<GeneratePostResponse> GenerateAsync(Guid userId, GeneratePostRequest request)
    {

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new NotFoundException("User");

        if (user.Credits <= 0)
            throw new InsufficientCreditsException();

        var output = await _openAiService.GenerateLinkedInPost(request.InputText, request.GoalType);

        user.Credits -= 1;

        var generation = new Generation
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            GoalType = request.GoalType,
            OutputText = output,
            InputText = request.InputText,
            CreatedAt = DateTime.UtcNow
        };

        _db.Generations.Add(generation);

        await _db.SaveChangesAsync();

        return new GeneratePostResponse
        {
            OutputText = output,
        };
    }

    public async Task<Pa>

}