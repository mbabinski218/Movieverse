using Movieverse.Domain.ValueObjects.Ids;

namespace Movieverse.Domain.Common.Models;

public abstract class AggregateRoot<TId, TIdType> : Entity<TId>, IAggregateRoot
	where TId : AggregateRootId<TIdType>
{
	public DateTimeOffset CreatedAt { get; set; }
	public DateTimeOffset? UpdatedAt { get; set; }

	private AggregateRootId<TIdType> _id = null!;
	public new AggregateRootId<TIdType> Id => _id;

	protected AggregateRoot(TId id)
	{
		_id = id;
	}

	protected AggregateRoot()
	{
		
	}
}