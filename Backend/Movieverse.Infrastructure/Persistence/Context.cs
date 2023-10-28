using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Movieverse.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Types;

namespace Movieverse.Infrastructure.Persistence;

public sealed class Context : IdentityDbContext<User, IdentityUserRole, Guid>
{
	// DbSet
	public DbSet<Media> Medias { get; set; } = null!;
	public DbSet<Movie> Movies { get; set; } = null!;
	public DbSet<Series> Series { get; set; } = null!;
	public DbSet<Content> Contents { get; set; } = null!;
	public DbSet<Genre> Genres { get; set; } = null!;
	public DbSet<Person> Persons { get; set; } = null!;
	public DbSet<Platform> Platforms { get; set; } = null!;

	// Configuration
	private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;
	private readonly DateTimeSetterInterceptor _dateTimeSetterInterceptor;
	
	public Context(DbContextOptions<Context> options, PublishDomainEventsInterceptor publishDomainEventsInterceptor,
		DateTimeSetterInterceptor dateTimeSetterInterceptor) : base(options)
	{
		_publishDomainEventsInterceptor = publishDomainEventsInterceptor;
		_dateTimeSetterInterceptor = dateTimeSetterInterceptor;
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.HasPostgresEnum<Role>();
		modelBuilder.Ignore<IdentityUserLogin<Guid>>();
		
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		modelBuilder.Entity<Media>().OwnsOne(m => m.Details);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);

		optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor, _dateTimeSetterInterceptor);
	}
}