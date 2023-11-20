using Microsoft.EntityFrameworkCore;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Infrastructure.Persistence;
using NSubstitute;

namespace Movieverse.UnitTests.InfrastructureUnitTests.Mocks;

public static class ReadOnlyContextMock
{
	public static ReadOnlyContext Get()
	{
		var options = new DbContextOptionsBuilder<ReadOnlyContext>().UseInMemoryDatabase("Movieverse").Options;
		var db = new ReadOnlyContext(options);
		
		db.Media = Substitute.For<DbSet<Media>>();
		db.Movies = Substitute.For<DbSet<Movie>>();
		db.Series = Substitute.For<DbSet<Series>>();
		db.Contents = Substitute.For<DbSet<Content>>();
		db.Persons = Substitute.For<DbSet<Person>>();
		db.Platforms = Substitute.For<DbSet<Platform>>();
		db.Users = Substitute.For<DbSet<User>>();

		return db;
	}
}