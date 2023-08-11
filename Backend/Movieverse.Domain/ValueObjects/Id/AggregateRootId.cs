namespace Movieverse.Domain.ValueObjects.Id;

public sealed class AggregateRootId : BaseEntityId<Guid>
{
	private AggregateRootId()
	{
		
	}
	
	private AggregateRootId(Guid value) : base(value)
	{
	}
	
	public static AggregateRootId Create(Guid value) => new(value);

	public static implicit operator AggregateRootId(Guid value) => Create(value);
}