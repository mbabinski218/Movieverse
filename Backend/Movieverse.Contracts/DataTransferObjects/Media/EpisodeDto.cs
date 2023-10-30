using Movieverse.Domain.ValueObjects;

namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class EpisodeDto
{
	public short EpisodeNumber { get; set; }
	public string Title { get; set; } = null!;
	public Details Details { get; set; } = null!;
	//public List<Guid> ContentIds { get; private set; } = new();
	//public List<ReviewDto> Reviews { get; private set; } = new();
}