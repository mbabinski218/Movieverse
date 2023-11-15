using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Types;
using Movieverse.Domain.Entities;

namespace Movieverse.Infrastructure.Persistence;

public sealed class ReadOnlyContext : IdentityDbContext<User, IdentityUserRole, Guid>
{
	// DbSet
	public DbSet<Media> Media { get; set; } = null!;
	public DbSet<Movie> Movies { get; set; } = null!;
	public DbSet<Series> Series { get; set; } = null!;
	public DbSet<Genre> Genres { get; set; } = null!;
	public DbSet<Content> Contents { get; set; } = null!;
	public DbSet<Person> Persons { get; set; } = null!;
	public DbSet<Platform> Platforms { get; set; } = null!;
	
	// Configuration
	public ReadOnlyContext(DbContextOptions<ReadOnlyContext> options) : base(options)
	{
		ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
		ChangeTracker.AutoDetectChangesEnabled = false;
	}
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		
		modelBuilder.HasPostgresEnum<Role>();
		modelBuilder.Ignore<IdentityUserLogin<Guid>>();
		
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
	
	private const string readOnlyErrorMessage = "Context is read-only.";
	
	#pragma warning disable CS0809
	[Obsolete(readOnlyErrorMessage, true)]
	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		throw new InvalidOperationException(readOnlyErrorMessage);
	}

	[Obsolete(readOnlyErrorMessage, true)]
	public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
	{
		throw new InvalidOperationException(readOnlyErrorMessage);
	}

	[Obsolete(readOnlyErrorMessage, true)]
	public override int SaveChanges()
	{
		throw new InvalidOperationException(readOnlyErrorMessage);
	}

	[Obsolete(readOnlyErrorMessage, true)]
	public override int SaveChanges(bool acceptAllChangesOnSuccess)
	{
		throw new InvalidOperationException(readOnlyErrorMessage);
	}
	#pragma warning restore CS0809
}