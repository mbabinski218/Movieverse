namespace Movieverse.Domain.Common.Types;

public enum SortType
{
	ReleaseDateAscending,
	ReleaseDateDescending,
	PopularityAscending,
	PopularityDescending,
	VotesAscending,
	VotesDescending
}

public static class SortTypeHelper
{
	public static string ToSqlString(this SortType sortType)
	{
		return sortType switch
		{
			SortType.ReleaseDateAscending => "ReleaseDate ASC",
			SortType.ReleaseDateDescending => "ReleaseDate DESC",
			SortType.PopularityAscending => "Popularity ASC",
			SortType.PopularityDescending => "Popularity DESC",
			SortType.VotesAscending => "Votes ASC",
			SortType.VotesDescending => "Votes DESC",
			_ => throw new ArgumentOutOfRangeException(nameof(sortType), sortType, null)
		};
		
	}
	
	public static SortType ToSortType(this string str) => Enum.Parse<SortType>(str);

	public static IEnumerable<string> Supported() => Enum.GetNames<SortType>();
}