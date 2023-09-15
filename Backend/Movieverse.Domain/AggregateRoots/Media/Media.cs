using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.AggregateRoots.Media;

public class Media : AggregateRoot<MediaId, Guid>
{
	// Map to table
	//private readonly List<PlatformId> _platformIds = new();
	
	public string Title { get; set; } = null!;
	public Details Details { get; set; } = null!;
	public TechnicalSpecs TechnicalSpecs { get; set; } = null!;
	public int CurrentPosition { get; set; }
	public BasicStatistics BasicStatistics { get; set; } = null!;
	public virtual Statistics AdvancedStatistics { get; set; } = null!;
	public ContentId? PosterId { get; set; }
	public ContentId? TrailerId { get; set; }
	// public IReadOnlyList<PlatformId> PlatformIds => _platformIds.AsReadOnly();
	public virtual List<PlatformId> PlatformIds { get; private set; } = new();
	public virtual List<ContentId> ContentIds { get; private set; } = new();
	public virtual List<GenreId> GenreIds { get; private set; } = new();
	public virtual List<Review> Reviews { get; private set; } = new();
	public virtual List<Staff> Staff { get; private set; } = new();

	// EF Core
	protected Media()
	{
		
	}
	
	protected Media(MediaId id, string title) : base(id)
	{
		Title = title;
		CurrentPosition = 0;
		BasicStatistics = new BasicStatistics();
	}
	
	public void AddPlatformId(PlatformId platformId)
	{
		// _platformIds.Add(platformId);
		PlatformIds.Add(platformId);
	}
}