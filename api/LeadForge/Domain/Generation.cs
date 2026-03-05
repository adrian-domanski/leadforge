using System.ComponentModel.DataAnnotations;
using LeadForge.Domain.Enums;

namespace LeadForge.Domain;

public class Generation
{
   public Guid Id { get; set; }
   public Guid UserId { get; set; }

   public string InputText { get; set; } = string.Empty;

   public string OutputText { get; set; } = string.Empty;

   public GoalType GoalType { get; set; }
   public DateTime CreatedAt { get; set; }

   // Navigation property
   public User User { get; set; } = null!;
}