﻿using Microsoft.EntityFrameworkCore;
using Movieverse.Domain.Common;

namespace Movieverse.Application.Common;

public sealed class PaginatedList<TKey> : IPaginatedList<TKey>
{
	public List<TKey> Items { get; init; } = null!;
	public short? PageNumber { get; init; }
	public short? TotalPages { get; init; }
	public short TotalCount { get; init; }
	public bool HasPreviousPage => PageNumber > 1;
	public bool HasNextPage => PageNumber < TotalPages;

	public PaginatedList(List<TKey> items, short? pageNumber, short? pageSize)
	{
		Items = items;
		PageNumber = pageNumber;
		
		var count = (short)items.Count;
		TotalPages = pageSize == null ? null : (short)Math.Ceiling(count / (double)pageSize.Value);
		TotalCount = count;
	}

	private PaginatedList()
	{
		
	}
	
	public static async Task<PaginatedList<TKey>> CreateAsync(IQueryable<TKey> source, short? pageNumber, short? pageSize, 
		CancellationToken cancellationToken = default)
	{
		var count = (short)await source.CountAsync(cancellationToken);

		if (pageSize.HasValue && pageNumber.HasValue)
		{
			source = source.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
		}

		var items = await source.ToListAsync(cancellationToken);

		return new PaginatedList<TKey>
		{
			Items = items,
			PageNumber = pageNumber,
			TotalPages = pageSize == null ? null : (short)Math.Ceiling(count / (double)pageSize.Value),
			TotalCount = count
		};
	}
    
	public static PaginatedList<TKey> Create(IEnumerable<TKey> source, short? pageNumber, short? pageSize)
	{
		var temp = source.ToList();
		var count = (short)temp.Count;
        
		if (pageSize.HasValue && pageNumber.HasValue)
		{
			temp = temp.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
		}
        
		return new PaginatedList<TKey>
		{
			Items = temp,
			PageNumber = pageNumber,
			TotalPages = pageSize == null ? null : (short)Math.Ceiling(count / (double)pageSize.Value),
			TotalCount = count
		};
	}
}

public static class PaginatedListExtensions
{
	public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable,
		short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
		where TDestination : class
		=> PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize, cancellationToken);

	public static PaginatedList<TDestination> ToPaginatedList<TDestination>(this IEnumerable<TDestination> enumerable,
		short? pageNumber, short? pageSize)
		where TDestination : class
		=> PaginatedList<TDestination>.Create(enumerable, pageNumber, pageSize);
}