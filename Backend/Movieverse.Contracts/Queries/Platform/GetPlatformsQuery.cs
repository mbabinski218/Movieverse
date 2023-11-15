using MediatR;
using Movieverse.Contracts.DataTransferObjects.Platform;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.Platform;

public sealed record GetPlatformsQuery : IRequest<Result<IEnumerable<PlatformDemoDto>>>;