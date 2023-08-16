﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Infrastructure.Persistence;
using Movieverse.Infrastructure.Persistence.Interceptors;
using NSubstitute;

namespace Movieverse.UnitTests.InfrastructureUnitTests.Mocks;

public static class AppDbContextMock
{
	public static AppDbContext Get()
	{
		var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("Movieverse").Options;
		
		var publishDomainEventsInterceptor = new PublishDomainEventsInterceptor(
			Substitute.For<IPublisher>(), Substitute.For<ILogger<PublishDomainEventsInterceptor>>());
		
		var dateTimeSetterInterceptor = new DateTimeSetterInterceptor(Substitute.For<ILogger<DateTimeSetterInterceptor>>());
		
		var logger = Substitute.For<ILogger<AppDbContext>>();

		var db = new AppDbContext(options, publishDomainEventsInterceptor, dateTimeSetterInterceptor, logger);
		
		db.Medias = Substitute.For<DbSet<Media>>();
		db.Movies = Substitute.For<DbSet<Movie>>();
		db.Series = Substitute.For<DbSet<Series>>();
		db.Contents = Substitute.For<DbSet<Content>>();
		db.Genres = Substitute.For<DbSet<Genre>>();
		db.Persons = Substitute.For<DbSet<Person>>();
		db.Platforms = Substitute.For<DbSet<Platform>>();
		db.Users = Substitute.For<DbSet<User>>();

		return db;
	}
}