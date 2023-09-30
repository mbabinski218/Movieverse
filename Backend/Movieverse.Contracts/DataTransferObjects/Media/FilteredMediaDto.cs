using Movieverse.Domain.Common;

namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class FilteredMediaDto
{
	public Guid PlatformId { get; set; }
	public string PlatformName { get; set; } = null!;
	public IPaginatedList<MediaDemoDto> Media { get; set; } = null!;
}