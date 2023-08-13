namespace Movieverse.Domain.Common;

public interface IPaginatedList<TKey>
{
	List<TKey> Items { get; init; }
	int? PageNumber { get; init; }
	int? TotalPages { get; init; }
	int TotalCount { get; init; }
	bool HasPreviousPage => PageNumber > 1;
	bool HasNextPage => PageNumber < TotalPages;
}