using MediatR;
using Movieverse.Domain.DomainEvents;

namespace Movieverse.Application.EventHandlers;

public sealed class ImageChangedHandler : INotificationHandler<ImageChanged>
{
	public Task Handle(ImageChanged notification, CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}