using MediatR;
using Movieverse.Contracts.DataTransferObjects.Content;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.Media;

public sealed record GetContentPathQuery(
	Guid Id
	) : IRequest<Result<IEnumerable<ContentInfoDto>>>;