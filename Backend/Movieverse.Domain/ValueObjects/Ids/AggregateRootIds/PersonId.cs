namespace Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

public sealed class PersonId : AggregateRootId<Guid>
{
	private PersonId(Guid value) : base(value)
	{
		
	}	
	
	public static PersonId Create() => new(Guid.NewGuid());
	public static PersonId Create(Guid value) => new(value);
	public static PersonId Create(string value) => new(Guid.Parse(value));

	public static implicit operator PersonId(Guid value) => Create(value);
	
	public static implicit operator PersonId(string value) => Create(value);
}