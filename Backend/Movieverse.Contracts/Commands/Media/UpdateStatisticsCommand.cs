using MediatR;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Commands.Media;

public record UpdateStatisticsCommand() : IRequest<Result>;