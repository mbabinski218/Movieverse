namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class FilteredMediaDto
{
	public Guid? Id { get; set; }
	public string? Name { get; set; } = null!;
	public IEnumerable<MediaDemoDto> Medias { get; set; } = null!;
}