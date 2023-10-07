using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.ValueObjects;

public sealed class BasicStatistics : ValueObject
{
	public double Rating { get; set; }
	public int Votes { get; set; }
	public int UserReviews { get; set; }
	public int CriticReviews { get; set; }
	public int InWatchlistCount { get; set; }

	public BasicStatistics()
	{
		Rating = 0;
		Votes = 0;
		UserReviews = 0;
		CriticReviews = 0;
		InWatchlistCount = 0;
	}
	
	public static BasicStatistics operator +(BasicStatistics a, BasicStatistics b)
	{
		return new()
		{
			Rating = (short)(a.Rating + b.Rating),
			Votes = a.Votes + b.Votes,
			UserReviews = a.UserReviews + b.UserReviews,
			CriticReviews = a.CriticReviews + b.CriticReviews,
			InWatchlistCount = a.InWatchlistCount + b.InWatchlistCount
		};
	}
	
	public static BasicStatistics operator -(BasicStatistics a, BasicStatistics b)
	{
		return new()
		{
			Rating = (short)(a.Rating - b.Rating),
			Votes = a.Votes - b.Votes,
			UserReviews = a.UserReviews - b.UserReviews,
			CriticReviews = a.CriticReviews - b.CriticReviews,
			InWatchlistCount = a.InWatchlistCount - b.InWatchlistCount
		};
	}
}