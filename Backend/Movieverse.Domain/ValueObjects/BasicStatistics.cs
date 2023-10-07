using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.ValueObjects;

public sealed class BasicStatistics : ValueObject
{
	public decimal Rating { get; set; }
	public int Votes { get; set; }
	public int UserReviews { get; set; }
	public int CriticReviews { get; set; }
	public int OnWatchlistCount { get; set; }

	public BasicStatistics()
	{
		Rating = 0;
		Votes = 0;
		UserReviews = 0;
		CriticReviews = 0;
		OnWatchlistCount = 0;
	}
	
	public static BasicStatistics operator +(BasicStatistics a, BasicStatistics b)
	{
		return new()
		{
			Rating = (short)(a.Rating + b.Rating),
			Votes = a.Votes + b.Votes,
			UserReviews = a.UserReviews + b.UserReviews,
			CriticReviews = a.CriticReviews + b.CriticReviews,
			OnWatchlistCount = a.OnWatchlistCount + b.OnWatchlistCount
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
			OnWatchlistCount = a.OnWatchlistCount - b.OnWatchlistCount
		};
	}
}