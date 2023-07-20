using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.ValueObjects;

public class MediaInfo : ValueObject
{
	public ObjectId MediaId { get; set; } = null!;
	public bool IsInWatchlist { get; set; }
	public short Rating { get; set; }
}