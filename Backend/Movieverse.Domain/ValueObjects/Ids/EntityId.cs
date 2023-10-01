using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.ValueObjects.Ids;

public abstract class EntityId<TKey> : ValueObject
	where TKey : notnull
{
	public TKey Value { get; } = default!;
	
	protected EntityId()
	{
		
	}
	
	protected EntityId(TKey value)
	{
		Value = value;
	}
	
	public static implicit operator TKey(EntityId<TKey> id) => id.Value;
	public override string ToString() => Value.ToString() ?? throw new NullReferenceException(nameof(EntityId<TKey>));
}