using MediatR;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.Media;

public record GetSeasonsQuery(
	Guid Id
	) : IRequest<Result<SeasonInfoDto>>;