﻿using Movieverse.Domain.Entities;

namespace Movieverse.Domain.AggregateRoots.Media;

public class Series : Media
{
	// Map to table
	public virtual List<Season> Seasons { get; private set; } = new();
	public short SeasonCount { get; set; }
	public short EpisodeCount { get; set; }

	// EF Core
	private Series()
	{
		
	}
	
	// Other
	
}