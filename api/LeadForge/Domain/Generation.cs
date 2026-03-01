using System.ComponentModel.DataAnnotations;

namespace LeadForge.Domain;

public class Generation
{
   public Guid Id { get; set; }
   public Guid UserId { get; set; }

   public string InputText { get; set; } = string.Empty;

   public string OutputText { get; set; } = string.Empty;

   public string GoalType { get; set; } = string.Empty;
   public DateTime CreatedAt { get; set; }

   // Navigation property
   public User User { get; set; } = null!;
}