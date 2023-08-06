using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movieverse.Application.Interfaces;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Types;
using Movieverse.Infrastructure.Persistence;
using Movieverse.Infrastructure.Persistence.Interceptors;
using Movieverse.Infrastructure.Repositories;
using Npgsql;

namespace Movieverse.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddPersistence(configuration);
		services.AddAuthentication(configuration);
		services.AddRepositories();
		
		return services;
	}
	
	private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<AppDbContext>(options => options.UseNpgsql(GetNpgsqlDataSource()));

		services.AddIdentityCore<User>(options =>
			{
				options.User.RequireUniqueEmail = true;
				
				//TODO Remove password requirements
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequiredLength = 1;
				options.Password.RequiredUniqueChars = 0;
			})
			.AddRoles<IdentityUserRole>()
			.AddEntityFrameworkStores<AppDbContext>();
		
		services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<PublishDomainEventsInterceptor>();
		services.AddScoped<DateTimeSetterInterceptor>();
		
		return services;
	}

	private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		
		
		return services;
	}
	
	private static IServiceCollection AddRepositories(this IServiceCollection services)
	{
		services.AddScoped<IContentRepository, ContentRepository>();
		services.AddScoped<IGenreRepository, GenreRepository>();
		services.AddScoped<IMediaRepository, MediaRepository>();
		services.AddScoped<IPersonRepository, PersonRepository>();
		services.AddScoped<IPlatformRepository, PlatformRepository>();
		services.AddScoped<IUserRepository, UserRepository>();

		return services;
	}

	private static NpgsqlDataSource GetNpgsqlDataSource()
	{
		var dataSourceBuilder = new NpgsqlDataSourceBuilder("Host=localhost; database=Test; Username=postgres; Password=Babina123456789");

		dataSourceBuilder.MapEnum<Role>();

		return dataSourceBuilder.Build();
	}
}