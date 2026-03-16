using FluentValidation;

namespace LeadForge.Application.Validators;

public class GeneratePostRequestValidator : AbstractValidator<GeneratePostRequest>
{
   public GeneratePostRequestValidator()
   {
      RuleFor(x => x.InputText)
         .NotEmpty()
         .MinimumLength(10)
         .WithMessage("Give us more text to work with!");

      RuleFor(x => x.GoalType)
         .NotEmpty()
         .WithMessage("Goal type cannot be empty.");
   }
}