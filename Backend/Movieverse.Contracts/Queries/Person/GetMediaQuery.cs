using MediatR;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.Person;

public sealed record GetMediaQuery(
	Guid Id
	) : IRequest<Result<IEnumerable<MediaSectionDto>>>;