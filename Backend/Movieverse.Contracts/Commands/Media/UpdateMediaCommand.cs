using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Contracts.Commands.Media;

public sealed record UpdateMediaCommand(
	[FromRoute] Guid Id,
	string? Title,
	Details? Details,
	TechnicalSpecs? TechnicalSpecs,
	IFormFile? Poster,
	string? Trailer,
	IEnumerable<IFormFile>? ImagesToAdd,
	IEnumerable<string>? VideosToAdd,
	IEnumerable<Guid>? ContentToRemove,
	IEnumerable<Guid>? PlatformIds,
	IEnumerable<Guid>? GenreIds,
	IEnumerable<PostStaffDto>? Staff,
	MovieInfoDto? MovieInfo,
	SeriesInfoDto? SeriesInfo
	) : IRequest<Result<MediaDto>>;