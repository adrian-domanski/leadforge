using LeadForge.Domain.Enums;

namespace LeadForge.Application;

public class GenerationListItem
{
    public Guid Id { get; set; }
    public GoalType GoalType { get; set; }
    public string InputText { get; set; } = null!;
    public string OutputText { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}