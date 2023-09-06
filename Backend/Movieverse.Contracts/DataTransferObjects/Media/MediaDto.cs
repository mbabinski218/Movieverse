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
	public List<Guid> PlatformIds { get; private set; } = new();
	public List<Guid> ContentIds { get; private set; } = new();
	public List<Guid> GenreIds { get; private set; } = new();
	public List<ReviewDto> Reviews { get; private set; } = new();
	public List<StaffDto> Staff { get; private set; } = new();
}