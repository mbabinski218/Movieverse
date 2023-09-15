namespace Movieverse.Domain.ValueObjects.Ids;

public abstract class AggregateRootId<TKey> : EntityId<TKey>
{
	protected AggregateRootId(TKey value) : base(value)
	{
		
	}
}