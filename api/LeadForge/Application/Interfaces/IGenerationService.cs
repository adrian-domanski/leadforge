namespace LeadForge.Application.Interfaces;

public interface IGenerationService
{
    Task<GeneratePostResponse> GenerateAsync(
        Guid userId, string inputText, string goalType);

}