namespace LeadForge.Domain.Exceptions.OpenAi;

public class OpenAiQuotaExceededException : DomainException
{
    public OpenAiQuotaExceededException()
        : base("AI generation quota exceeded. Please try again later.")
    {
    }
}