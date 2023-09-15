using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.Entities;

public class Review : Entity<int>
{
	// Map to table
	public virtual Media Media { get; private set; } = null!;
	public Guid UserId { get; private set; }
	public string UserName { get; set; } = null!;
	public string Title { get; set; } = null!;
	public string Content { get; set; } = null!;
	public short Rating { get; set; } //update po evencie RatingChanged, przy tworzeniu ustawienie na wartość w userze
	public DateTimeOffset Date { get; private set; }
	public bool ByCritic { get; set; }
	public bool Spoiler { get; set; }
	public bool Modified { get; set; }
	public bool Deleted { get; set; }
	public bool Banned { get; set; }

	// EF Core
	private Review()
	{
		
	}
	
	// Other
}