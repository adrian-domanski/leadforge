using LeadForge.Application.Interfaces;
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

   public async Task<string> GenerateLinkedInPost(string input, string goal)
   {
      var client = new OpenAIClient(_apiKey);
      var chatClient = client.GetChatClient("gpt-4o-mini");

      var prompt = BuildPrompt(input, goal);

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

   private string BuildPrompt(string input, string goal)
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
              {input}
              """;
   }
}