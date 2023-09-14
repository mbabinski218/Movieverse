﻿using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.AggregateRoots.Media;

public class Media : AggregateRoot
{
	// Map to table
	private readonly List<AggregateRootId> _platformIds = new();
	
	public string Title { get; set; } = null!;
	public Details Details { get; set; } = null!;
	public TechnicalSpecs TechnicalSpecs { get; set; } = null!;
	public int CurrentPosition { get; set; }
	public BasicStatistics BasicStatistics { get; set; } = null!;
	public virtual Statistics AdvancedStatistics { get; set; } = null!;
	public AggregateRootId? PosterId { get; set; }
	public AggregateRootId? TrailerId { get; set; }
	public IReadOnlyList<AggregateRootId> PlatformIds => _platformIds.AsReadOnly();
	public virtual List<AggregateRootId> ContentIds { get; private set; } = new();
	public virtual List<AggregateRootId> GenreIds { get; private set; } = new();
	public virtual List<Review> Reviews { get; private set; } = new();
	public virtual List<Staff> Staff { get; private set; } = new();

	// EF Core
	protected Media()
	{
		
	}
	
	protected Media(AggregateRootId id, string title)
	{
		Id = id;
		Title = title;
		CurrentPosition = 0;
		BasicStatistics = new BasicStatistics();
	}
	
	public void AddPlatformId(AggregateRootId platformId)
	{
		_platformIds.Add(platformId);
	}
}