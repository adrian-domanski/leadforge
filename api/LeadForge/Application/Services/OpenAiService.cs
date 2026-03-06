using LeadForge.Application.Interfaces;
using LeadForge.Domain.Enums;
using OpenAI;
using OpenAI.Chat;

namespace LeadForge.Application;

public class OpenAiService : IOpenAiService
{
   private readonly string _apiKey;

   public OpenAiService(IConfiguration configuration)
   {
      _apiKey = configuration["OpenAI:ApiKey"]
                ?? throw new Exception("OpenAI API key not configured.");
   }

   public async Task<string> GenerateLinkedInPost(GenerateLinkedInPostRequest request,
      CancellationToken ct)
   {
      var client = new OpenAIClient(_apiKey);
      var chatClient = client.GetChatClient("gpt-4o-mini");

      var prompt = BuildPrompt(request.InputText, request.GoalType);

      var response = await chatClient.CompleteChatAsync(
         new ChatMessage[]
         {
            ChatMessage.CreateSystemMessage(
               "You are a LinkedIn content strategist for software agency founders."
            ),
            ChatMessage.CreateUserMessage(prompt)
         }
      );

      return response.Value.Content[0].Text;
   }

   private string BuildPrompt(string inputText, GoalType goal)
   {
      return $"""
              Write a LinkedIn post designed to generate inbound leads.

              Goal: {goal}

              Rules:
              - Strong hook (first line)
              - Short paragraphs
              - No fluff
              - Highlight business outcome
              - Professional tone
              - Soft CTA at the end

              Idea:
              {inputText}
              """;
   }


}