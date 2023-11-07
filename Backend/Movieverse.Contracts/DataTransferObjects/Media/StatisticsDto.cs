using Movieverse.Domain.ValueObjects;

namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class StatisticsDto
{
	public List<PopularityDto> Popularity { get; set; } = null!; 
	public BoxOffice BoxOffice { get; set; } = null!;
}