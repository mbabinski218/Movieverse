using MediatR;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries;

public sealed record TestQuery() : IRequest<Result>;