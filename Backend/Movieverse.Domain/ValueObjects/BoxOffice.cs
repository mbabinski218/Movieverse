using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.ValueObjects;

public class BoxOffice : ValueObject
{
	public decimal Budget { get; set; }
	public decimal Revenue { get; set; }
	public decimal GrossUs { get; set; }
	public decimal WorldwideGross { get; set; }
	public decimal OpeningWeekendUs { get; set; }
	public decimal OpeningWeekendWorldwide { get; set; }
	public int Theaters { get; set; }
}