using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Contracts.Queries.Media;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Media;

public sealed class GetMediaHandler : IRequestHandler<GetMediaQuery, Result<MediaDto>>
{
	private readonly ILogger<GetMediaHandler> _logger;
	private readonly IMediaRepository _mediaRepository;
	private readonly IMapper _mapper;

	public GetMediaHandler(ILogger<GetMediaHandler> logger, IMediaRepository mediaRepository, IMapper mapper)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
		_mapper = mapper;
	}

	public async Task<Result<MediaDto>> Handle(GetMediaQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Getting media with id {Id}", request.Id);
		
		var media = await _mediaRepository.FindAsync(request.Id, cancellationToken).ConfigureAwait(false);
		if (media.IsUnsuccessful)
		{
			return media.Error;
		}
		
		return media.Value switch
		{
			Movie movie => _mapper.Map<MovieDto>(movie),
			Series series => _mapper.Map<SeriesDto>(series),
			_ => Error.Invalid(MediaResources.InvalidMediaType)
		};
	}
}