using Movieverse.Domain.ValueObjects;

namespace Movieverse.Contracts.DataTransferObjects.Media;

public class MediaDto
{
	public string Title { get; set; } = null!;
	public Details Details { get; set; } = null!;
	public TechnicalSpecs TechnicalSpecs { get; set; } = null!;
	public int? CurrentPosition { get; set; }
	public BasicStatistics BasicStatistics { get; set; } = null!;
	public Guid? PosterId { get; set; }
	public Guid? TrailerId { get; set; }
	public List<Guid> PlatformIds { get; set; } = new();
	public List<Guid> ContentIds { get; set; } = new();
	public List<Guid> GenreIds { get; set; } = new();
	public ReviewDto? LatestReview { get; set; } = new();
	public List<StaffDto> Staff { get; set; } = new();
}