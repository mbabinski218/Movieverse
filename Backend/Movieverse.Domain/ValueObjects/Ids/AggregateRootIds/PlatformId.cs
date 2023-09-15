namespace Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

public sealed class PlatformId : AggregateRootId<Guid>
{
	private PlatformId(Guid value) : base(value)
	{
		
	}	
	
	public static PlatformId Create() => new(Guid.NewGuid());
	public static PlatformId Create(Guid value) => new(value);
	public static PlatformId Create(string value) => new(Guid.Parse(value));

	public static implicit operator PlatformId(Guid value) => Create(value);
	
	public static implicit operator PlatformId(string value) => Create(value);
}