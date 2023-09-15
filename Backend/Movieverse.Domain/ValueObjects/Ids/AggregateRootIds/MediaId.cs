namespace Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

public sealed class MediaId : AggregateRootId<Guid>
{
	private MediaId(Guid value) : base(value)
	{
		
	}	
	
	public static MediaId Create() => new(Guid.NewGuid());
	public static MediaId Create(Guid value) => new(value);
	public static MediaId Create(string value) => new(Guid.Parse(value));

	public static implicit operator MediaId(Guid value) => Create(value);
	
	public static implicit operator MediaId(string value) => Create(value);
}