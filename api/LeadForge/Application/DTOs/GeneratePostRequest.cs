using System.ComponentModel.DataAnnotations;

namespace LeadForge.Application;

public class GeneratePostRequest
{
    [Required]
    [MinLength(10)]
    public string InputText { get; set; } = null!;

    [Required]
    public string GoalType { get; set; } = null!;
}