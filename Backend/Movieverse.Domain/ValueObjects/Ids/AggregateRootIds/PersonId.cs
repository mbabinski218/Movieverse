namespace Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

public sealed class PersonId : AggregateRootId<Guid>
{
	// Constructors
	private PersonId(Guid value) : base(value)
	{
		
	}	
	
	// Methods
	public static PersonId Create() => new(Guid.NewGuid());
	public static PersonId Create(Guid value) => new(value);

	// Operators
	public static implicit operator PersonId(Guid value) => Create(value);
	public static explicit operator Guid(PersonId personId) => personId.Value;
}