using Movieverse.Domain.Common;

namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class FilteredMediaDto
{
	public string PlaceName { get; set; } = null!;
	public Guid? PlaceId { get; set; }
	public IPaginatedList<MediaDemoDto> Medias { get; set; } = null!;
}