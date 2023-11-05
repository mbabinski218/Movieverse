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
}