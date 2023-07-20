namespace Movieverse.Domain.Common.Models;

public abstract class Entity<TKey> : IHasDomainEvent
	where TKey : ValueObject
{
	public TKey Id { get; set; } = default!;
	
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