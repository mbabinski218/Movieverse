namespace Movieverse.Contracts.DataTransferObjects.User;

public sealed class WatchlistStatusDto
{
	public Guid MediaId { get; set; }
	public bool IsOnWatchlist { get; set; }
}