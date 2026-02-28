namespace LeadForge.Application.Interfaces;

public interface IOpenAiService
{
   Task<string> GenerateLinkedInPost(string input, string goal);
}