namespace LeadForge.Application;

public class MeResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public int Credits { get; set; }
}