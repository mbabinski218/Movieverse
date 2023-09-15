using System.ComponentModel.DataAnnotations.Schema;

namespace Movieverse.Domain.Common.Models;

public class Entity<TId> : IEquatable<Entity<TId>>, IHasDomainEvent
	where TId : notnull
{
	public TId Id { get; protected set; }
    
	private readonly List<IDomainEvent> _domainEvents = new();
	
	[NotMapped]
	public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

	protected Entity(TId id)
	{
		Id = id;
	}

	protected Entity()
	{
		
	}
	
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
	
	public void ClearDomainEvents()
	{
		_domainEvents.Clear();
	}
	
	public void AddDomainEvent(IDomainEvent domainEvent)
	{
		_domainEvents.Add(domainEvent);
	}
}