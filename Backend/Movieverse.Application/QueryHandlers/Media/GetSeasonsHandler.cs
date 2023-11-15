using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Contracts.Queries.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Media;

public sealed class GetSeasonsHandler : IRequestHandler<GetSeasonsQuery, Result<SeasonInfoDto>>
{
	private readonly ILogger<GetSeasonsHandler> _logger;
	private readonly IMediaReadOnlyRepository _mediaRepository;

	public GetSeasonsHandler(ILogger<GetSeasonsHandler> logger, IMediaReadOnlyRepository mediaRepository)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
	}

	public async Task<Result<SeasonInfoDto>> Handle(GetSeasonsQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Retrieving seasons info for media with id {Id}", request.Id);
		
		return await _mediaRepository.GetSeasonsAsync(request.Id, cancellationToken);
	}
}