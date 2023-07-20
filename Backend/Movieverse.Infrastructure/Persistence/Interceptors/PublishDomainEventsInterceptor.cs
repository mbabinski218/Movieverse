using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Movieverse.Domain.Common;
using MediatR;

namespace Movieverse.Infrastructure.Persistence.Interceptors;

public class PublishDomainEventsInterceptor : SaveChangesInterceptor
{
	private readonly IPublisher _mediator;

	public PublishDomainEventsInterceptor(IPublisher mediator)
	{
		_mediator = mediator;
	}

	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		PublishDomainEventsAsync(eventData.Context).GetAwaiter().GetResult();
		return base.SavingChanges(eventData, result);
	}
	
	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new())
	{
		await PublishDomainEventsAsync(eventData.Context);
		return await base.SavingChangesAsync(eventData, result, cancellationToken);
	}
	
	private async Task PublishDomainEventsAsync(DbContext? context)
	{
		if (context is null)
		{
			return;
		}

		var domainEvents = context.ChangeTracker
			.Entries<IHasDomainEvent>()
			.Where(entry => entry.Entity.DomainEvents.Any())
			.Select(entry => entry.Entity)
			.SelectMany(entity =>
			{
				var domainEvents = entity.DomainEvents;
				entity.ClearDomainEvents();
				return domainEvents;
			})
			.ToList();

		foreach (var domainEvent in domainEvents)
		{
			await _mediator.Publish(domainEvent);
		}
	}
}