using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.ValueObjects;

public sealed class BoxOffice : ValueObject
{
	public decimal Budget { get; set; }
	public decimal Revenue { get; set; }
	public decimal GrossUs { get; set; }
	public decimal GrossWorldwide { get; set; }
	public decimal OpeningWeekendUs { get; set; }
	public decimal OpeningWeekendWorldwide { get; set; }
	public ushort Theaters { get; set; }

	private BoxOffice()
	{
		
	}
}