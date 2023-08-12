using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Movieverse.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Types;

namespace Movieverse.Infrastructure.Persistence;

public sealed class AppDbContext : IdentityDbContext<User, IdentityUserRole, Guid>, IAppDbContext
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
	
	private readonly ILogger<AppDbContext> _logger;
	
	public AppDbContext(DbContextOptions<AppDbContext> options, PublishDomainEventsInterceptor publishDomainEventsInterceptor, 
		DateTimeSetterInterceptor dateTimeSetterInterceptor, ILogger<AppDbContext> logger) : base(options)
	{
		_publishDomainEventsInterceptor = publishDomainEventsInterceptor;
		_dateTimeSetterInterceptor = dateTimeSetterInterceptor;
		_logger = logger;
	}
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		_logger.LogDebug("Model building...");

		base.OnModelCreating(modelBuilder);
		
		modelBuilder.HasPostgresEnum<Role>();

		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		_logger.LogDebug("Configuring database...");
				
		base.OnConfiguring(optionsBuilder);
		
		optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor, _dateTimeSetterInterceptor);
	}
}