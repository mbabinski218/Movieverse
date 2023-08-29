using FluentValidation;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.Genre;
using Movieverse.Domain.Common;

namespace Movieverse.Application.CommandHandlers.GenreCommands;

public sealed class AddGenreValidator : AbstractValidator<AddGenreCommand>
{
	public AddGenreValidator()
	{
		RuleFor(g => g.Name)
			.NotEmpty().WithMessage(GenreResources.EnterName)
			.MaximumLength(Constants.nameLength).WithMessage(GenreResources.NameTooLong);

		RuleFor(g => g.Description)
			.NotEmpty().WithMessage(GenreResources.EnterDescription)
			.MaximumLength(Constants.descriptionLength).WithMessage(GenreResources.DescriptionTooLong);
	}
}