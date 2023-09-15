namespace Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

public sealed class ContentId : AggregateRootId<Guid>
{
	private ContentId(Guid value) : base(value)
	{
		
	}	
	
	public static ContentId Create() => new(Guid.NewGuid());
	public static ContentId Create(Guid value) => new(value);
	public static ContentId Create(string value) => new(Guid.Parse(value));

	public static implicit operator ContentId(Guid value) => Create(value);
	
	public static implicit operator ContentId(string value) => Create(value);
}