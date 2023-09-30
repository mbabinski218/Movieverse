namespace Movieverse.Contracts.DataTransferObjects.Platform;

public sealed class PlatformDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = null!; 
	public Guid? LogoId { get; set; }
	public decimal Price { get; set; }
}