namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class StaffDto
{
	public Guid PersonId { get; set; }
	public string Role { get; set; } = null!;
}