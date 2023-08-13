using MediatR;
using Movieverse.Application.Common.Result;

namespace Movieverse.Application.Commands.MediaCommands.UpdateStatistics;

public record UpdateStatisticsCommand() : IRequest<Result>;