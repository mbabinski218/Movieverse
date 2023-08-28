using FluentValidation;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.User;

namespace Movieverse.Application.CommandHandlers.UserCommands.Update;

public sealed class UpdateValidator : AbstractValidator<UpdateCommand>
{
	public UpdateValidator()
	{
		RuleFor(u => u.Email)
			.EmailAddress().WithMessage(UserResources.WrongEmailFormat);
		
		RuleFor(u => u.Information)
			.Must(i => i?.Age > 0)
			.When(u => u.Information?.Age != null)
			.WithMessage(UserResources.AgeMustBeGreaterThanZero);
	}
}