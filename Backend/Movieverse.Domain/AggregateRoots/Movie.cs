using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.AggregateRoots;

public class Movie : Media
{
	public virtual Movie? Sequel { get; set; }
	public ObjectId? SequelId { get; set; }
	public virtual Movie? Prequel { get; set; }
	public ObjectId? PrequelId { get; set; }
}