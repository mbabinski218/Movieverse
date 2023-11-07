namespace Movieverse.Contracts.DataTransferObjects.Person;

public sealed class PersonInfoDto
{
	public Guid Id { get; set; }
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public Guid? PictureId { get; set; }
}