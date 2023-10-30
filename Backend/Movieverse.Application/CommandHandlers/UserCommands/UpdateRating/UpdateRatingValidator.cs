using FluentValidation;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.User;

namespace Movieverse.Application.CommandHandlers.UserCommands.UpdateRating;

public sealed class UpdateRatingValidator : AbstractValidator<UpdateRatingCommand>
{
	public UpdateRatingValidator()
	{
		RuleFor(u => u.Rating)
			.InclusiveBetween((ushort)1, (ushort)10).WithMessage("Rating must be between 1 and 10");
	}
}