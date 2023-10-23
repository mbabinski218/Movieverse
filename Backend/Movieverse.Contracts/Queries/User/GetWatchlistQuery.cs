using MediatR;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.User;

public sealed record GetWatchlistQuery(
	short? PageNumber,
	short? PageSize
	): IRequest<Result<IPaginatedList<SearchMediaDto>>>;