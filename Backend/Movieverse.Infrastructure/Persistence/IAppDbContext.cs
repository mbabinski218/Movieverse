using Microsoft.EntityFrameworkCore;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.AggregateRoots.Media;

namespace Movieverse.Infrastructure.Persistence;

public interface IAppDbContext
{
	DbSet<Media> Medias { get; set; }
	DbSet<Movie> Movies { get; set; }
	DbSet<Series> Series { get; set; }
	DbSet<Content> Contents { get; set; }
	DbSet<Genre> Genres { get; set; }
	DbSet<Person> Persons { get; set; }
	DbSet<Platform> Platforms { get; set; }
	DbSet<User> Users { get; set; }
}