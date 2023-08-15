﻿using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Contracts.Commands.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.CommandHandlers.MediaCommands.UpdateStatistics;

public sealed class UpdateStatisticsHandler : IRequestHandler<UpdateStatisticsCommand, Result>
{
	private readonly ILogger<UpdateStatisticsHandler> _logger;
	private readonly IMediaRepository _mediaRepository;

	public UpdateStatisticsHandler(ILogger<UpdateStatisticsHandler> logger, IMediaRepository mediaRepository)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
	}

	public async Task<Result> Handle(UpdateStatisticsCommand request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Updating statistics...");
		
		var result = await _mediaRepository.UpdateStatisticsAsync();
		
		if (!result.IsSuccessful)
		{
			_logger.LogError("Statistics update failed: {error}", string.Join(",", result.Error.Messages));
		}

		return result;
	}
}