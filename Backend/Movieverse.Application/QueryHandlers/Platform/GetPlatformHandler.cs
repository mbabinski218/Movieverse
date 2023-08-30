using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Contracts.DataTransferObjects.Platform;
using Movieverse.Contracts.Queries.Platform;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Platform;

public sealed class GetPlatformHandler : IRequestHandler<GetPlatformQuery, Result<PlatformDto>>
{
	private readonly ILogger<GetPlatformHandler> _logger;
    private readonly IPlatformRepository _platformRepository;
    private readonly IMapper _mapper;

    public GetPlatformHandler(ILogger<GetPlatformHandler> logger, IPlatformRepository platformRepository, IMapper mapper)
    {
	    _logger = logger;
	    _platformRepository = platformRepository;
	    _mapper = mapper;
    }

    public async Task<Result<PlatformDto>> Handle(GetPlatformQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Getting platform {id}...", request.Id);
		
		var platform = await _platformRepository.FindByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
		return platform.IsSuccessful ? _mapper.Map<PlatformDto>(platform.Value) : platform.Error;
	}
}