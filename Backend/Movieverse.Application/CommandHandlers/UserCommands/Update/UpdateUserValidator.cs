using FluentValidation;
using Movieverse.Contracts.Commands.User;

namespace Movieverse.Application.CommandHandlers.UserCommands.Update;

public sealed class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
	public UpdateUserValidator()
	{
		RuleFor(u => u.Email)
			.EmailAddress().WithMessage("Wrong email format");
		
		RuleFor(u => u.Information)
			.Must(i => i?.Age > 0)
			.When(u => u.Information?.Age != null)
			.WithMessage("Age must be greater than 0");
	}
}