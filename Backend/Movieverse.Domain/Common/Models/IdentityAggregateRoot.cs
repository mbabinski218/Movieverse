using Microsoft.AspNetCore.Identity;

namespace Movieverse.Domain.Common.Models;

public abstract class IdentityAggregateRoot : IdentityUser<Guid>, IHasDomainEvent
{
	private readonly List<IDomainEvent> _domainEvents = new();
	public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

	protected IdentityAggregateRoot()
	{
		
	}
	
	public void ClearDomainEvents()
	{
		_domainEvents.Clear();
	}

	public void AddDomainEvent(IDomainEvent domainEvent)
	{
		_domainEvents.Add(domainEvent);
	}
}