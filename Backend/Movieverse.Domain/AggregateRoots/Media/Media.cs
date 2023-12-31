﻿using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.AggregateRoots.Media;

public class Media : AggregateRoot<MediaId, Guid>
{
	// Map to table
	private readonly List<PlatformId> _platformIds = new();
	private readonly List<ContentId> _contentIds = new();
	private readonly List<Staff> _staff = new();
	private readonly List<Review> _reviews = new();
	private readonly List<Genre> _genres = new();
	public string Title { get; set; } = null!;
	public Details Details { get; set; } = null!;
	public TechnicalSpecs TechnicalSpecs { get; set; } = null!;
	public int CurrentPosition { get; set; }
	public BasicStatistics BasicStatistics { get; set; } = null!;
	public virtual Statistics AdvancedStatistics { get; set; } = null!;
	public ContentId? PosterId { get; set; }
	public ContentId? TrailerId { get; set; }
	public IReadOnlyList<PlatformId> PlatformIds => _platformIds.AsReadOnly();
	public IReadOnlyList<ContentId> ContentIds => _contentIds.AsReadOnly();
	public IReadOnlyList<Staff> Staff => _staff.AsReadOnly();
	public IReadOnlyList<Review> Reviews => _reviews.AsReadOnly();
	public IReadOnlyList<Genre> Genres => _genres.AsReadOnly();
	
	
	// Constructors
	protected Media(MediaId id, string title) : base(id)
	{
		Title = title;
		CurrentPosition = 0;
		BasicStatistics = new BasicStatistics();
	}
	
	// Methods
	public void AddPlatform(PlatformId platformId)
	{
		_platformIds.Add(platformId);
	}
	
	public void AddContent(ContentId contentId)
	{
		_contentIds.Add(contentId);
	}
	
	public void RemoveContent(ContentId contentId)
	{
		_contentIds.Remove(contentId);
	}
	
	public void AddStaff(Staff staff)
	{
		_staff.Add(staff);
	}
	
	public void RemoveStaff(Staff staff)
	{
		_staff.Remove(staff);
	}

	public void AddReview(Review review)
	{
		_reviews.Add(review);
	}

	public void AddGenre(Genre genre)
	{
		_genres.Add(genre);
	}

	public void ClearGenres()
	{
		_genres.Clear();
	}

	public void ClearPlatforms()
	{
		_platformIds.Clear();
	}

	public void ClearStaff()
	{
		_staff.Clear();
	}
	
	// Equality
	public override bool Equals(object? obj) => obj is Media entity && Id.Equals(entity.Id);
	public override int GetHashCode() => Id.GetHashCode();
	
	// EF Core
	protected Media()
	{
		
	}
}