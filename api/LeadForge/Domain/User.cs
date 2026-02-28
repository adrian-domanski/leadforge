namespace LeadForge.Domain;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public int Credits { get; set; } = 5;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public ICollection<Generation> Generations { get; set; } = new List<Generation>();
}