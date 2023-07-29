namespace Movieverse.Domain.ValueObjects.Id;

public sealed class EntityId : BaseEntityId<int>
{
	private EntityId()
	{
		
	}
	
	private EntityId(int value) : base(value)
	{
	}
	
	public static EntityId Create(int value) => new(value);
}