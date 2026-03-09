using LeadForge.Application.Interfaces;
using LeadForge.Domain.Enums;
using LeadForge.Domain.Exceptions.OpenAi;
using OpenAI;
using OpenAI.Chat;

namespace LeadForge.Application;

public class OpenAiService : IOpenAiService
{
   private readonly ChatClient _chatClient;

   public OpenAiService(IConfiguration configuration)
   {
      var apiKey = configuration["OpenAI:ApiKey"]
                ?? throw new Exception("OpenAI API key not configured.");

      var client = new OpenAIClient(apiKey);

      _chatClient = client.GetChatClient("gpt-4o-mini");
   }

   public async Task<string> GenerateLinkedInPost(
      GenerateLinkedInPostRequest request,
      CancellationToken ct)
   {
      try
      {
         var prompt = BuildPrompt(request.InputText, request.GoalType);

         var response = await _chatClient.CompleteChatAsync(
            new ChatMessage[]
            {
               ChatMessage.CreateSystemMessage(
                  "You are a LinkedIn content strategist helping software agency founders generate inbound leads."
               ),
               ChatMessage.CreateUserMessage(prompt)
            },
            cancellationToken: ct
         );

         var content = response.Value.Content.FirstOrDefault()?.Text;

         if (string.IsNullOrWhiteSpace(content))
            throw new Exception("OpenAI returned empty response.");

         return content;
      }
      catch (Exception ex)
      {
         if (ex.Message.Contains("insufficient_quota"))
            throw new OpenAiQuotaExceededException();

         throw;
      }
   }

   private string BuildPrompt(string inputText, GoalType goal)
   {
      var goalInstruction = goal switch
      {
         GoalType.LeadGeneration =>
            "Focus on attracting potential clients and encouraging them to reach out.",

         GoalType.Authority =>
            "Position the author as an expert and share valuable insights from experience.",

         GoalType.Storytelling =>
            "Tell a short personal or business story with a lesson relevant to founders.",

         GoalType.Engagement =>
            "Encourage discussion and comments by asking thoughtful questions.",

         _ =>
            "Write a high-quality LinkedIn post."
      };

      return $"""
              Write a LinkedIn post.

              Objective:
              {goalInstruction}

              Rules:
              - Strong hook in the first line
              - Short paragraphs
              - No fluff
              - Highlight business outcomes
              - Professional tone
              - Maximum 200 words
              - End with a soft call-to-action

              Idea for the post:
              {inputText}
              """;
   }


}