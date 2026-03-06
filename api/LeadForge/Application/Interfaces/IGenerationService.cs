namespace LeadForge.Application.Interfaces;

public interface IGenerationService
{
    Task<GeneratePostResponse> GenerateAsync( GeneratePostRequest request, CancellationToken ct);
    Task<PagedResult<GenerationListItem>> GetUserGenerationsAsync(
        int page,
        int pageSize,
        CancellationToken ct);

    Task<GenerationListItem> GetByIdAsync(
        Guid id,
        CancellationToken ct);
}