using System.ComponentModel.DataAnnotations.Schema;

namespace Movieverse.Domain.Common.Models;

public class Entity<TId> : IEquatable<Entity<TId>>, IHasDomainEvent
	where TId : notnull
{
	// Map to table
	public TId Id { get; } = default!;
    
	private readonly List<IDomainEvent> _domainEvents = new();
	
	[NotMapped]
	public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

	// Constructors
	protected Entity(TId id)
	{
		Id = id;
	}
	
	// Equality
	public override bool Equals(object? obj)
	{
		return obj is Entity<TId> entity && Id.Equals(entity.Id);
	}

	public bool Equals(Entity<TId>? other)
	{
		return Equals((object?)other);
	}

	public static bool operator ==(Entity<TId> left, Entity<TId> right)
	{
		return Equals(left, right);
	}

	public static bool operator !=(Entity<TId> left, Entity<TId> right)
	{
		return !Equals(left, right);
	}

	public override int GetHashCode()
	{
		return Id.GetHashCode();
	}
	
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
	protected Entity()
	{
		
	}
}