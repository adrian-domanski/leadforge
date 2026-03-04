namespace LeadForge.Domain.Exceptions;

public class AlreadyExistsException : DomainException
{
    public AlreadyExistsException(string message) : base(message)
    {
    }
}