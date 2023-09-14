using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.ValueObjects.Id;

public abstract class BaseEntityId<TKey> : ValueObject
{
	public TKey Value { get; } = default!;
	
	protected BaseEntityId()
	{
		
	}
	
	protected BaseEntityId(TKey value)
	{
		Value = value;
	}
	
	public static implicit operator TKey(BaseEntityId<TKey> id) => id.Value; 
}