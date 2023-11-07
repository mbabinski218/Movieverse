using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.AggregateRoots.Media;

public class Media : AggregateRoot<MediaId, Guid>
{
	// Map to table
	private readonly List<PlatformId> _platformIds = new();
	private readonly List<ContentId> _contentIds = new();
	private readonly List<GenreId> _genreIds = new();
	private readonly List<Staff> _staff = new();

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
	public IReadOnlyList<GenreId> GenreIds => _genreIds.AsReadOnly();
	public IReadOnlyList<Staff> Staff => _staff.AsReadOnly();
	
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
	
	public void AddGenre(GenreId genreId)
	{
		_genreIds.Add(genreId);
	}
	
	public void AddStaff(Staff staff)
	{
		_staff.Add(staff);
	}

	// Equality
	public override bool Equals(object? obj) => obj is MediaId entityId && Id.Equals(entityId);
	public override int GetHashCode() => Id.GetHashCode();
	
	// EF Core
	protected Media()
	{
		
	}
}