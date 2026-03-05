namespace LeadForge.Application.Interfaces;

public interface IGenerationService
{
    Task<GeneratePostResponse> GenerateAsync(Guid userId, GeneratePostRequest request);
}