using FluentValidation;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.User;

namespace Movieverse.Application.CommandHandlers.UserCommands.Register;

public sealed class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
	public RegisterUserValidator()
	{
		RuleFor(u => u.Email)
			.NotEmpty().WithMessage(UserResources.EnterEmail)
			.EmailAddress().WithMessage(UserResources.WrongEmailFormat);

		RuleFor(u => u.UserName)
			.NotEmpty().WithMessage(UserResources.EnterUserName);

		RuleFor(u => u.Password)
			.NotEmpty().WithMessage(UserResources.EnterPassword)
			.Equal(u => u.ConfirmPassword).WithMessage(UserResources.PasswordsMustBeTheSame);

		RuleFor(u => u.ConfirmPassword)
			.NotEmpty().WithMessage(UserResources.ConfirmPassword);
		
		RuleFor(u => u.Age)
			.NotEmpty().WithMessage(UserResources.EnterAge)
			.Must(a => a > 0).WithMessage(UserResources.AgeMustBeGreaterThanZero);
	}
}