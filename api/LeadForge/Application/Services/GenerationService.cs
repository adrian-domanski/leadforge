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

    public async Task<GeneratePostResponse> GenerateAsync(Guid userId, string inputText, string goalType)
    {

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new NotFoundException("User");

        if (user.Credits <= 0)
            throw new InsufficientCreditsException();

        var output = await _openAiService.GenerateLinkedInPost(inputText, goalType);

        user.Credits -= 1;

        var generation = new Generation
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            GoalType = goalType,
            OutputText = output,
            InputText = inputText
        };

        _db.Generations.Add(generation);

        await _db.SaveChangesAsync();

        return new GeneratePostResponse
        {
            Output = output,
            RemainingCredits = user.Credits
        };
    }
}