using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.Entities;

public class MediaInfo : Entity<ObjectId>
{
	public ObjectId MediaId { get; set; } = null!;
	public bool IsInWatchlist { get; set; }
	public short Rating { get; set; }
}