using MediatR;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.User;

public sealed record GetWatchlistStatusesQuery(
	IEnumerable<Guid> MediaIds
	) : IRequest<Result<IEnumerable<WatchlistStatusDto>>>;