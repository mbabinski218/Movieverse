namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class PopularityDto
{
	public DateTimeOffset Date { get; set; }
	public int Position { get; set; }
	public int Change { get; set; }
	public long Views { get; set; }
}