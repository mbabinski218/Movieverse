using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Movieverse.Application.Behaviors;

public sealed class LoggingBehavior<TRequest> : IRequestPreProcessor<TRequest> 
	where TRequest : notnull
{
	private readonly ILogger<LoggingBehavior<TRequest>> _logger;

	public LoggingBehavior(ILogger<LoggingBehavior<TRequest>> logger)
	{
		_logger = logger;
	}

	public Task Process(TRequest request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Request: {request}", request);
		
		return Task.CompletedTask;
	}
}