using System.ComponentModel.DataAnnotations.Schema;

namespace Movieverse.Domain.Common.Models;

public class BaseEntity<TKey> : IHasDomainEvent
	where TKey : notnull
{
	public TKey Id { get; } = default!;
    
	private readonly List<IDomainEvent> _domainEvents = new();
	
	[NotMapped]
	public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

	protected BaseEntity(TKey id)
	{
		Id = id;
	}

	protected BaseEntity()
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