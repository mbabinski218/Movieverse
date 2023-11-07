namespace Movieverse.Contracts.DataTransferObjects.User;

public sealed class MediaInfoDto
{
	public bool IsOnWatchlist { get; set; }
	public ushort Rating { get; set; }
}