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

	public BoxOffice()
	{
		Budget = 0;
		Revenue = 0;
		GrossUs = 0;
		GrossWorldwide = 0;
		OpeningWeekendUs = 0;
		OpeningWeekendWorldwide = 0;
		Theaters = 0;
	}
}