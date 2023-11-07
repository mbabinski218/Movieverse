using Movieverse.Domain.ValueObjects;

namespace Movieverse.Contracts.DataTransferObjects.Person;

public sealed class PersonDto
{
	public Information Information { get; set; } = null!;
	public LifeHistory LifeHistory { get; set; } = null!;
	public string? Biography { get; set; }
	public string? FunFacts { get; set; }
	public Guid? PictureId { get; set; }
	public List<Guid> ContentIds { get; set; } = new();
}