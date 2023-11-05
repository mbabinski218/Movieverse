using Movieverse.Domain.ValueObjects;

namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class StatisticsDto
{
	public BoxOffice BoxOffice { get; set; } = null!;
}