using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.ValueObjects;

public sealed class BasicStatistics : ValueObject
{
	public short Rating { get; set; }
	public int Votes { get; set; }
	public int UserReviews { get; set; }
	public int CriticReviews { get; set; }
	public int InWatchlistCount { get; set; }

	private BasicStatistics()
	{
		
	}
}