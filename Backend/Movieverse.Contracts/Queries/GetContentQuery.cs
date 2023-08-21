using MediatR;
using Movieverse.Contracts.DataTransferObjects.Content;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries;

public record GetContentQuery(Guid Id) : IRequest<Result<ContentDto>>;