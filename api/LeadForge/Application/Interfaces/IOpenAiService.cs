using LeadForge.Domain.Enums;

namespace LeadForge.Application.Interfaces;

public interface IOpenAiService
{
   Task<string> GenerateLinkedInPost(GenerateLinkedInPostRequest request);
}