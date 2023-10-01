namespace Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

public sealed class GenreId : AggregateRootId<Guid>
{
	// Constructors
	private GenreId(Guid value) : base(value)
	{
		
	}	
	
	// Methods
	public static GenreId Create() => new(Guid.NewGuid());
	public static GenreId Create(Guid value) => new(value);
	
	// Operators
	public static implicit operator GenreId(Guid value) => Create(value);
	public static explicit operator Guid(GenreId id) => id.Value;
}