using Microsoft.EntityFrameworkCore;
using Movieverse.Domain.Common;

namespace Movieverse.Application.Common;

public sealed class PaginatedList<TKey> : IPaginatedList<TKey>
{
	public List<TKey> Items { get; init; } = null!;
	public int? PageNumber { get; init; }
	public int? TotalPages { get; init; }
	public int TotalCount { get; init; }
	public bool HasPreviousPage => PageNumber > 1;
	public bool HasNextPage => PageNumber < TotalPages;

	public static async Task<PaginatedList<TKey>> CreateAsync(IQueryable<TKey> source, int? pageNumber, int? pageSize)
	{
		var count = await source.CountAsync();

		if (pageSize.HasValue && pageNumber.HasValue)
		{
			source = source.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
		}

		var items = await source.ToListAsync();

		return new PaginatedList<TKey>
		{
			Items = items,
			PageNumber = pageNumber,
			TotalPages = pageSize == null ? null : (int)Math.Ceiling(count / (double)pageSize.Value),
			TotalCount = count
		};
	}
    
	public static PaginatedList<TKey> Create(IEnumerable<TKey> source, int? pageNumber, int? pageSize)
	{
		var temp = source.ToList();
		var count = temp.Count;
        
		if (pageSize.HasValue && pageNumber.HasValue)
		{
			temp = temp.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
		}
        
		return new PaginatedList<TKey>
		{
			Items = temp,
			PageNumber = pageNumber,
			TotalPages = pageSize == null ? null : (int)Math.Ceiling(count / (double)pageSize.Value),
			TotalCount = count
		};
	}
}

public static class PaginatedListExtensions
{
	public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable,
		int? pageNumber, int? pageSize)
		where TDestination : class
		=> PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);

	public static PaginatedList<TDestination> ToPaginatedList<TDestination>(this IEnumerable<TDestination> enumerable,
		int? pageNumber, int? pageSize)
		where TDestination : class
		=> PaginatedList<TDestination>.Create(enumerable, pageNumber, pageSize);
}