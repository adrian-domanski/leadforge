namespace LeadForge.Application;

public class GeneratePostResponse
{

   public Guid Id { get; set; }
   public string InputText { get; set; } = string.Empty;
   public string OutputText { get; set; } = string.Empty;
   public DateTime CreatedAt { get; set; }
}