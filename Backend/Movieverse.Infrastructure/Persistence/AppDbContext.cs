using System.Reflection;
using Movieverse.Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Movieverse.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common;
using Movieverse.Domain.Entities;

namespace Movieverse.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<User, IdentityRole<ObjectId>, ObjectId>
{
	// DbSets
	public DbSet<Content> Contents { get; set; } = null!;
	public DbSet<Media> Medias { get; set; } = null!;
	public DbSet<Movie> Movies { get; set; } = null!;
	public DbSet<Person> Persons { get; set; } = null!;
	public DbSet<Platform> Platforms { get; set; } = null!;
	public DbSet<Series> Series { get; set; } = null!;
	public DbSet<Statistics> Statistics { get; set; } = null!;
	public DbSet<Award> Awards { get; set; } = null!;
	public DbSet<Episode> Episodes { get; set; } = null!;
	public DbSet<Genre> Genres { get; set; } = null!;
	public DbSet<MediaGenre> MediaGenres { get; set; } = null!;
	public DbSet<Popularity> Popularity { get; set; } = null!;
	public DbSet<Review> Reviews { get; set; } = null!;
	public DbSet<Season> Seasons { get; set; } = null!;
	public DbSet<StatisticsAward> StatisticsAwards { get; set; } = null!;

	// Configuration
	private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;
	
	public AppDbContext(DbContextOptions<AppDbContext> options, PublishDomainEventsInterceptor publishDomainEventsInterceptor) : base(options)
	{
		_publishDomainEventsInterceptor = publishDomainEventsInterceptor;
        
		// Database.EnsureDeleted();
		Database.EnsureCreated();
	}
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Ignore<List<IDomainEvent>>()
			.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
		base.OnModelCreating(modelBuilder);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor);
		base.OnConfiguring(optionsBuilder);
	}
}