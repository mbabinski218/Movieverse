namespace Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

public sealed class MediaId : AggregateRootId<Guid>
{
	// Constructors
	private MediaId(Guid value) : base(value)
	{
		
	}	
	
	// Methods
	public static MediaId Create() => new(Guid.NewGuid());
	public static MediaId Create(Guid value) => new(value);

	// Operators
	public static implicit operator MediaId(Guid value) => Create(value);
	public static explicit operator Guid(MediaId mediaId) => mediaId.Value;
	// public override AggregateRootId<Guid> AsAggregateRootId() => this;
}