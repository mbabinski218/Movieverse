using FluentValidation;

namespace Movieverse.Application.Commands.UserCommands.Register;

public sealed class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
	public RegisterUserValidator()
	{
		RuleFor(u => u.Email)
			.NotEmpty().WithMessage("Enter email address")
			.EmailAddress().WithMessage("Wrong email format");

		RuleFor(u => u.UserName)
			.NotEmpty().WithMessage("Enter username");

		RuleFor(u => u.Password)
			.NotEmpty().WithMessage("Enter password")
			.Equal(u => u.ConfirmPassword).WithMessage("Passwords must be the same");

		RuleFor(u => u.ConfirmPassword)
			.NotEmpty().WithMessage("Confirm password");
		
		RuleFor(u => u.Age)
			.NotEmpty().WithMessage("Enter age")
			.Must(a => a > 0).WithMessage("Age must be greater than 0");
	}
}