namespace Movieverse.Contracts.DataTransferObjects.Platform;

public sealed class PlatformInfoDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = null!; 
	public decimal Price { get; set; }
}