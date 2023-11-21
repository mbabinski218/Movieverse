using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.Entities;

public class Review : Entity<int>
{
	// Map to table
	public virtual Media Media { get; set; } = null!;
	public Guid UserId { get; private set; }
	public string UserName { get; set; } = null!;
	public string Text { get; set; } = null!;
	public DateTimeOffset Date { get; private set; }
	public bool Banned { get; set; }
	
	// Constructors
	private Review(Media media, Guid userId, string userName, string text)
	{
		Media = media;
		UserId = userId;
		UserName = userName;
		Text = text;
		Date = DateTimeOffset.UtcNow;
		Banned = false;
	}
	
	// Methods
	public static Review Create(Media media, Guid userId, string userName, string text) => new(media, userId, userName, text);
	
	// EF Core
	private Review()
	{
		
	}
}