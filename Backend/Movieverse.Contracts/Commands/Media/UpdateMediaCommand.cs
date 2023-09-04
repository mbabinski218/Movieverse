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
	IEnumerable<IFormFile>? Images,
	IEnumerable<string>? Videos,
	IEnumerable<StaffDto>? Staff,
	IEnumerable<Guid>? PlatformIds,
	MovieInfoDto? MovieInfo,
	SeriesInfoDto? SeriesInfo
	) : IRequest<Result<MediaDto>>;