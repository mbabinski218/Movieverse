using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.Entities;

public class MediaInfo : Entity<int>
{
	// Map to table
	public User User { get; private set; } = null!;
	public MediaId MediaId { get; private set; } = null!;
	public bool IsOnWatchlist { get; set; }
	public ushort Rating { get; set; }

	// Constructors
	private MediaInfo(User user, MediaId mediaId, bool isOnWatchlist, ushort rating)
	{
		User = user;
		MediaId = mediaId;
		IsOnWatchlist = isOnWatchlist;
		Rating = rating;
	}
	
	// Methods
	public static MediaInfo Create(User user, MediaId mediaId, bool isOnWatchlist, ushort rating)
		=> new(user, mediaId, isOnWatchlist, rating);
	
	// EF Core
	private MediaInfo()
	{
			
	}
}
