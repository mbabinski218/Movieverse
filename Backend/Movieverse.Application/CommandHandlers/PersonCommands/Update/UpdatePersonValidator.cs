using FluentValidation;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.Person;
using Movieverse.Domain.Common;

namespace Movieverse.Application.CommandHandlers.PersonCommands.Update;

public sealed class UpdatePersonValidator : AbstractValidator<UpdatePersonCommand>
{
	public UpdatePersonValidator()
	{
		RuleFor(p => p.Information!.FirstName)
			.Must(s => s!.Length > 0 && s!.Length <= Constants.nameLength)
			.When(p => p.Information != null && p.Information.FirstName != null)
			.WithMessage(PersonResources.FirstNameIsTooLong);
		
		RuleFor(p => p.Information!.LastName)
			.Must(s => s!.Length > 0 && s!.Length <= Constants.nameLength)
			.When(p => p.Information != null && p.Information.LastName != null)
			.WithMessage(PersonResources.LastNameIsTooLong);
		
		RuleFor(p => p.Information!.Age)
			.Must(s => s > 0)
			.When(p => p.Information != null)
			.WithMessage(PersonResources.AgeMustBeGreaterThanZero);
		
		RuleFor(p => p.Biography)
			.Must(s => s!.Length > 0 && s!.Length <= Constants.descriptionLength)
			.When(p => p.Biography != null)
			.WithMessage(PersonResources.BiographyIsTooLong);

		RuleFor(p => p.FunFacts)
			.Must(s => s!.Length > 0 && s!.Length <= Constants.descriptionLength)
			.When(p => p.FunFacts != null)
			.WithMessage(PersonResources.FunFactsAreTooLong);
	}
}