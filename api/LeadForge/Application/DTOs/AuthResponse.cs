namespace LeadForge.Application;

public class AuthResponse
{
   public string AccessToken { get; set; } = null!;
   public string RefreshToken { get; set; } = null!;
}