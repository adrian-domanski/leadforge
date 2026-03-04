namespace LeadForge.Domain.Exceptions;

public class InvalidCredentialsException : DomainException
{
    public InvalidCredentialsException(string message) : base("Invalid credentials.")
    {
    }
}