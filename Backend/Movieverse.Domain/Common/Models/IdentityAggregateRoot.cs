using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Movieverse.Domain.Common.Models;

public abstract class IdentityAggregateRoot : IdentityUser<Guid>, IAggregateRoot, IHasDomainEvent
{
	// Map to table
	public DateTimeOffset CreatedAt { get; set; }
	public DateTimeOffset? UpdatedAt { get; set; }
	
	private readonly List<IDomainEvent> _domainEvents = new();
	
	[NotMapped]
	public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
	
	// Methods
	public void ClearDomainEvents()
	{
		_domainEvents.Clear();
	}

	public void AddDomainEvent(IDomainEvent domainEvent)
	{
		_domainEvents.Add(domainEvent);
	}
	
	// EF Core
	protected IdentityAggregateRoot()
	{
		
	}
}