using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Platform;
using Movieverse.Contracts.Queries.Platform;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Platform;

public sealed class GetPlatformHandler : IRequestHandler<GetPlatformQuery, Result<PlatformDto>>
{
	private readonly ILogger<GetPlatformHandler> _logger;
    private readonly IPlatformReadOnlyRepository _platformRepository;
    public GetPlatformHandler(ILogger<GetPlatformHandler> logger, IPlatformReadOnlyRepository platformRepository)
    {
	    _logger = logger;
	    _platformRepository = platformRepository;
    }

    public async Task<Result<PlatformDto>> Handle(GetPlatformQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Getting platform {id}...", request.Id);
		
		var platform = await _platformRepository.FindAsync(request.Id, cancellationToken);
		return platform.IsSuccessful ? platform.Value : platform.Error;
	}
}