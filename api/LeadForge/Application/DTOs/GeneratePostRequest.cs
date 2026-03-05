using System.ComponentModel.DataAnnotations;
using LeadForge.Domain.Enums;

namespace LeadForge.Application;

public class GeneratePostRequest
{
    public string InputText { get; set; } = null!;

    public GoalType GoalType { get; set; }
}