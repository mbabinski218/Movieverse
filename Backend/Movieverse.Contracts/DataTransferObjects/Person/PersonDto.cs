using Movieverse.Domain.ValueObjects;

namespace Movieverse.Contracts.DataTransferObjects.Person;

public sealed class PersonDto
{
	public Information Information { get; set; } = null!;
	public LifeHistory LifeHistory { get; set; } = null!;
	public string? Biography { get; set; }
	public string? FunFacts { get; set; }
	public List<Guid> PictureIds { get; set; } = new();
}