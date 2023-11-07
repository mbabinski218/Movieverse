using MediatR;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.Media;

public sealed record GetStatisticsQuery(
	Guid Id
	) : IRequest<Result<StatisticsDto>>;