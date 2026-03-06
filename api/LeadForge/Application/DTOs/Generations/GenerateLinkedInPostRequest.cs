using LeadForge.Domain.Enums;

namespace LeadForge.Application;

public class GenerateLinkedInPostRequest
{
   public string InputText { get; set; } = null!;
   public GoalType GoalType { get; set; }
}