using MediatR;
using Movieverse.Contracts.DataTransferObjects.Platform;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries;

public sealed record GetPlatformQuery(Guid Id) : IRequest<Result<PlatformDto>>;