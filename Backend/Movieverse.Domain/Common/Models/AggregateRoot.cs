using Movieverse.Domain.ValueObjects.Ids;

namespace Movieverse.Domain.Common.Models;

public abstract class AggregateRoot<TId, TIdType> : Entity<TId>, IAggregateRoot
	where TId : AggregateRootId<TIdType>
	where TIdType : notnull
{
	// Map to table
	public new AggregateRootId<TIdType> Id { get; } = null!;
	public DateTimeOffset CreatedAt { get; set; }
	public DateTimeOffset? UpdatedAt { get; set; }

	// Constructors
	protected AggregateRoot(TId id)
	{
		Id = id;
	}

	// EF Core
	protected AggregateRoot()
	{
		
	}
}