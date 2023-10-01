namespace Movieverse.Domain.Common;

public interface IPaginatedList<TKey>
{
	List<TKey> Items { get; init; }
	short? PageNumber { get; init; }
	short? TotalPages { get; init; }
	short TotalCount { get; init; }
	bool HasPreviousPage => PageNumber > 1;
	bool HasNextPage => PageNumber < TotalPages;
}