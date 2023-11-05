using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Contracts.Queries.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Media;

public sealed class GetStatisticsHandler : IRequestHandler<GetStatisticsQuery, Result<StatisticsDto>>
{
	private readonly ILogger<GetStatisticsHandler> _logger;
	private readonly IMediaReadOnlyRepository _mediaRepository;

	public GetStatisticsHandler(ILogger<GetStatisticsHandler> logger, IMediaReadOnlyRepository mediaRepository)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
	}

	public async Task<Result<StatisticsDto>> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Retrieving statistics for media with id {Id}", request.Id);

		return await _mediaRepository.GetStatisticsAsync(request.Id, cancellationToken);
	}
}