namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class StaffDto
{
	public Guid PersonId { get; set; }
	public string Role { get; set; } = null!;
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public Guid? PictureId { get; set; }
}