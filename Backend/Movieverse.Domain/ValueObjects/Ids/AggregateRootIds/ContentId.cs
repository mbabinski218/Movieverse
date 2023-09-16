namespace Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

public sealed class ContentId : AggregateRootId<Guid>
{
	// Constructors
	private ContentId(Guid value) : base(value)
	{
		
	}	
	
	// Methods
	public static ContentId Create() => new(Guid.NewGuid());
	public static ContentId Create(Guid value) => new(value);

	// Operators
	public static implicit operator ContentId(Guid value) => Create(value);
	public static explicit operator Guid(ContentId id) => id.Value;
}