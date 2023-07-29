using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.ValueObjects.Id;

public class BaseEntityId<TKey> : ValueObject
{
	public TKey Value { get; private set; } = default!;
	
	protected BaseEntityId()
	{
		
	}
	
	protected BaseEntityId(TKey value)
	{
		Value = value;
	}
	
	public static implicit operator TKey(BaseEntityId<TKey> id) => id.Value; 
}