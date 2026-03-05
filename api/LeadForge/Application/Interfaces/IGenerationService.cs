namespace LeadForge.Application.Interfaces;

public interface IGenerationService
{
    Task<GeneratePostResponse> GenerateAsync( GeneratePostRequest request);
}