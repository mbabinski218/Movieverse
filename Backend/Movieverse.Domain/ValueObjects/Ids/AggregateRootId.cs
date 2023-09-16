namespace Movieverse.Domain.ValueObjects.Ids;

public abstract class AggregateRootId<TKey> : EntityId<TKey>
	where TKey : notnull
{
	protected AggregateRootId(TKey value) : base(value)
	{
		
	}

	public AggregateRootId<TKey> AsAggregateRootId() => this;
}