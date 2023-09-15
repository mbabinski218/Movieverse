namespace Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

public sealed class GenreId : AggregateRootId<Guid>
{
	private GenreId(Guid value) : base(value)
	{
		
	}	
	
	public static GenreId Create() => new(Guid.NewGuid());
	public static GenreId Create(Guid value) => new(value);
	public static GenreId Create(string value) => new(Guid.Parse(value));

	public static implicit operator GenreId(Guid value) => Create(value);
	
	public static implicit operator GenreId(string value) => Create(value);
}