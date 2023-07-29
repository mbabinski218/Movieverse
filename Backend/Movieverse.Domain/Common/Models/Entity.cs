namespace Movieverse.Domain.Common.Models;

public abstract class Entity : BaseEntity<int>
{
	protected Entity(int id) : base(id)
	{
		
	}

	protected Entity()
	{
		
	}
}