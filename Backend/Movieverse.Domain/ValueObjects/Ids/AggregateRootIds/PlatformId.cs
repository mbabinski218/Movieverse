namespace Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

public sealed class PlatformId : AggregateRootId<Guid>
{
	// Constructors
	private PlatformId(Guid value) : base(value)
	{
		
	}	
	
	// Methods
	public static PlatformId Create() => new(Guid.NewGuid());
	public static PlatformId Create(Guid value) => new(value);

	// Operators
	public static implicit operator PlatformId(Guid value) => Create(value);
	public static explicit operator Guid(PlatformId platformId) => platformId.Value;
}