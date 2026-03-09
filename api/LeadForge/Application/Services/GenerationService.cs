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
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<GenerationService> _logger;

    public GenerationService(AppDbContext db, IOpenAiService openAiService, ICurrentUserService currentUserService, ILogger<GenerationService> logger)
    {
        _db = db;
        _openAiService = openAiService;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public async Task<GeneratePostResponse> GenerateAsync( GeneratePostRequest request,
        CancellationToken ct)
    {

        var userId = _currentUserService.GetUserId();
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);

        if (user == null)
            throw new NotFoundException("User");

        _logger.LogInformation("User {UserId} requested post generation with goal {GoalType}" ,
            userId, request.GoalType);

        if (user.Credits <= 0)
        {
            _logger.LogWarning("User {UserId} attempted generation without credits.", userId);
            throw new InsufficientCreditsException();
        }

        // Generate linked in post with AI
        var output = await _openAiService.GenerateLinkedInPost(new GenerateLinkedInPostRequest
        {
          GoalType  = request.GoalType,
          InputText = request.InputText
        },ct);

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

        await _db.SaveChangesAsync(ct);

        _logger.LogInformation("Generation {GenerationId} created for user {UserId}", generation
            .Id, userId);

        return new GeneratePostResponse
        {
            Id = generation.Id,
            OutputText = output,
            CreatedAt = generation.CreatedAt,
            InputText = generation.InputText
        };
    }

    public async Task<PagedResult<GenerationListItem>> GetUserGenerationsAsync(int page, int
            pageSize, CancellationToken ct)
    {
        var userId = _currentUserService.GetUserId();

        var query = _db.Generations
            .Where(g => g.UserId == userId)
            .OrderByDescending(g => g.CreatedAt);

        var total = await query.CountAsync(ct);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(g => new GenerationListItem
            {
                Id = g.Id,
                GoalType = g.GoalType,
                InputText = g.InputText,
                OutputText = g.OutputText,
                CreatedAt = g.CreatedAt
            })
            .AsNoTracking()
            .ToListAsync(ct);

        return new PagedResult<GenerationListItem>
        {
            Items = items,
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };

    }

    public async Task<GenerationListItem> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var userId = _currentUserService.GetUserId();

        var generation = await _db.Generations
            .Where(g => g.Id == id && g.UserId == userId)
            .Select(g => new GenerationListItem
            {
                Id = g.Id,
                GoalType = g.GoalType,
                InputText = g.InputText,
                OutputText = g.OutputText,
                CreatedAt = g.CreatedAt
            })
            .AsNoTracking()
            .SingleOrDefaultAsync(ct);

        if (generation == null)
            throw new NotFoundException("Generation");

        return generation;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var userId = _currentUserService.GetUserId();

        var generation = await _db.Generations
            .FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId, ct);

        if (generation == null)
            throw new NotFoundException("Generation");

        _db.Generations.Remove(generation);

        await _db.SaveChangesAsync(ct);
    }
}