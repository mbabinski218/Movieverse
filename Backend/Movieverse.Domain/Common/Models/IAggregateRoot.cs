namespace Movieverse.Domain.Common.Models;

public interface IAggregateRoot
{
	DateTimeOffset CreatedAt { get; set; }
	DateTimeOffset? UpdatedAt { get; set; }
}