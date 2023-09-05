using FluentValidation;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.Media;
using Movieverse.Domain.Common;

namespace Movieverse.Application.CommandHandlers.MediaCommands.Add;

public sealed class AddMediaValidator : AbstractValidator<AddMediaCommand>
{
	public AddMediaValidator()
	{
		RuleFor(m => m.Title)
			.MinimumLength(1).WithMessage(MediaResources.TitleTooShort)
			.MaximumLength(Constants.titleLength).WithMessage(MediaResources.TitleTooLong);
	}
}