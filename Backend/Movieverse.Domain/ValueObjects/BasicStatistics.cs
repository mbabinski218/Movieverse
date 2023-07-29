using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.ValueObjects;

public sealed class BasicStatistics : ValueObject
{
	public ushort Rating { get; set; }
	public uint Votes { get; set; }
	public uint UserReviews { get; set; }
	public uint CriticReviews { get; set; }
	public uint InWatchlistCount { get; set; }

	private BasicStatistics()
	{
		
	}
}