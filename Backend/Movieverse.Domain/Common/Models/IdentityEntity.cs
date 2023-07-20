using Microsoft.AspNetCore.Identity;

namespace Movieverse.Domain.Common.Models;

public abstract class IdentityEntity<TKey> : IdentityUser<TKey>, IHasDomainEvent
	where TKey : IEquatable<TKey>
{
	private readonly List<IDomainEvent> _domainEvents = new();
	public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
	
	public void ClearDomainEvents()
	{
		_domainEvents.Clear();
	}

	public void AddDomainEvent(IDomainEvent domainEvent)
	{
		_domainEvents.Add(domainEvent);
	}
}