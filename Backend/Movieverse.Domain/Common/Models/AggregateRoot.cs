namespace Movieverse.Domain.Common.Models;

public abstract class AggregateRoot : BaseEntity<Guid>
{
	public DateTimeOffset CreatedAt { get; set; }
	public DateTimeOffset? UpdatedAt { get; set; }

	protected AggregateRoot(Guid id) : base(id)
	{
		
	}

	protected AggregateRoot()
	{
		
	}
}