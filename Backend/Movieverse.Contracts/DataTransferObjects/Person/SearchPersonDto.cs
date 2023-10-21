namespace Movieverse.Contracts.DataTransferObjects.Person;

public sealed class SearchPersonDto
{
	public Guid Id { get; set; }
	public string FullName { get; set; } = null!;
	public ushort? Age { get; set; }
	public string? Picture { get; set; }
	public string? Biography { get; set; }
}