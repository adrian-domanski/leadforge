namespace LeadForge.Domain.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string entityName) : base($"{entityName} was not found.")
    {
    }
}