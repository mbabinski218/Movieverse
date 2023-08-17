using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Movieverse.Domain.Common;
using MediatR;

namespace Movieverse.Infrastructure.Persistence.Interceptors;

public sealed class PublishDomainEventsInterceptor : SaveChangesInterceptor
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
		await PublishDomainEventsAsync(eventData.Context, cancellationToken);
		return await base.SavingChangesAsync(eventData, result, cancellationToken);
	}
	
	private async Task PublishDomainEventsAsync(DbContext? context, CancellationToken cancellationToken = default)
	{
		if (context is null) return;

		var entitiesWithDomainEvents = context.ChangeTracker.Entries<IHasDomainEvent>()
			.Where(entry => entry.Entity.DomainEvents.Any())
			.Select(entry => entry.Entity)
			.ToList();
		
		var domainEvents = entitiesWithDomainEvents
			.SelectMany(entity => entity.DomainEvents)
			.ToList();
		
		entitiesWithDomainEvents.ForEach(entity => entity.ClearDomainEvents());

		foreach (var domainEvent in domainEvents)
		{
			await _mediator.Publish(domainEvent, cancellationToken);
		}
	}
}