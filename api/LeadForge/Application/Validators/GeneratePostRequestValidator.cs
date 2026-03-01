using FluentValidation;

namespace LeadForge.Application.Validators;

public class GeneratePostRequestValidator : AbstractValidator<GeneratePostRequest>
{
   public GeneratePostRequestValidator()
   {
      RuleFor(x => x.InputText)
         .NotEmpty()
         .MinimumLength(10)
         .WithMessage("Input text must be at least 10 characters long.");

      RuleFor(x => x.GoalType)
         .NotEmpty()
         .WithMessage("Goal type cannot be empty.");
   }
}