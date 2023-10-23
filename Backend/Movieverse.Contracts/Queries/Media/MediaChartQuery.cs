using MediatR;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.Media;

public sealed record MediaChartQuery(
	string? Type,
	string? Category,
	short? PageNumber,
	short? PageSize
	) : IRequest<Result<IPaginatedList<SearchMediaDto>>>;