namespace LeadForge.Domain.Exceptions;

public class InsufficientCreditsException : DomainException
{
   public InsufficientCreditsException() : base("User does not have enough credits.")
   {

   }
}