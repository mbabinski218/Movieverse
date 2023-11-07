using MediatR;
using Movieverse.Contracts.DataTransferObjects.Platform;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.Media;

public sealed record GetPlatformQuery(
	Guid Id
	) : IRequest<Result<IEnumerable<PlatformInfoDto>>>;