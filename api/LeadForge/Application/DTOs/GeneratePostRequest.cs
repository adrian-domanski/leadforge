using System.ComponentModel.DataAnnotations;

namespace LeadForge.Application;

public class GeneratePostRequest
{
    public string InputText { get; set; } = null!;

    public string GoalType { get; set; } = null!;
}