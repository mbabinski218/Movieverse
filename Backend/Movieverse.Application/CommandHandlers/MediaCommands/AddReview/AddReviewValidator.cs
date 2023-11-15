using FluentValidation;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.Media;
using Movieverse.Domain.Common;

namespace Movieverse.Application.CommandHandlers.MediaCommands.AddReview;

public sealed class AddReviewValidator : AbstractValidator<AddReviewCommand>
{
	public AddReviewValidator()
	{
		RuleFor(m => m.Text)
			.MinimumLength(1).WithMessage(MediaResources.TitleTooShort)
			.MaximumLength(Constants.reviewLength).WithMessage(MediaResources.TitleTooLong);
	}
}