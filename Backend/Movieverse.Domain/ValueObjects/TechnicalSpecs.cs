using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.ValueObjects;

public sealed class TechnicalSpecs : ValueObject
{
	public string? Color { get; set; }
	public string? AspectRatio { get; set; }
	public string? SoundMix { get; set; }
	public string? Camera { get; set; }
	public string? NegativeFormat { get; set; }

	private TechnicalSpecs()
	{
		
	}
}