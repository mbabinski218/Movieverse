using NetEscapades.EnumGenerators;

namespace Movieverse.Domain.Common.Types;

[EnumExtensions]
public enum SortType
{
	ReleaseDateAscending,
	ReleaseDateDescending,
	PopularityAscending,
	PopularityDescending,
	VotesAscending,
	VotesDescending
}

public static partial class SortTypeExtensions
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
			_ => string.Empty
		};
	}
}