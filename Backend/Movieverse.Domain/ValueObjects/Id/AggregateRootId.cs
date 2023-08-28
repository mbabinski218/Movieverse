namespace Movieverse.Domain.ValueObjects.Id;

public sealed class AggregateRootId : BaseEntityId<Guid>
{
	private AggregateRootId()
	{
		
	}
	
	private AggregateRootId(Guid value) : base(value)
	{
		
	}
	
	public static AggregateRootId Create() => new(Guid.NewGuid());
	public static AggregateRootId Create(Guid value) => new(value);
	public static AggregateRootId Create(string value) => new(Guid.Parse(value));

	public static implicit operator AggregateRootId(Guid value) => Create(value);
	public static implicit operator AggregateRootId(string value) => Create(value);

	public override string ToString() => Value.ToString();
}