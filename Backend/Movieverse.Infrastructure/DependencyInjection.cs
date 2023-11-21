using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Movieverse.Application.Common.Extensions;
using Movieverse.Application.Common.Settings;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Types;
using Movieverse.Infrastructure.Authentication;
using Movieverse.Infrastructure.Persistence;
using Movieverse.Infrastructure.Persistence.Interceptors;
using Movieverse.Infrastructure.Repositories;
using Npgsql;

namespace Movieverse.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, string databaseName)
	{
		services.AddSettings(configuration);
		services.AddPersistence(configuration, databaseName);
		services.AddAuthentication(configuration);
		services.AddRepositories();
		
		return services;
	}
	
	private static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
	{
		services.BindSettings<AuthenticationSettings>(configuration);
		
		return services;
	}
	
	private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration, string databaseName)
	{
		var connectionString = configuration.GetConnectionString(databaseName);
		ArgumentNullException.ThrowIfNull(connectionString, nameof(connectionString));
		
		var dbDataSource = GetNpgsqlDataSource(connectionString); // DO NOT MOVE TO UseNpgsql DIRECTLY! - it will generate exception "More than twenty 'IServiceProvider' instances..."
		
		services.AddDbContext<Context>(options => options.UseNpgsql(dbDataSource));
		services.AddDbContext<ReadOnlyContext>(options => options.UseNpgsql(dbDataSource));
		
		services.AddHealthChecks().AddNpgSql(connectionString, name:"database");

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

				options.SignIn.RequireConfirmedEmail = true;
			})
			.AddRoles<IdentityUserRole>()
			.AddDefaultTokenProviders()
			.AddEntityFrameworkStores<Context>();
        
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();
		services.AddScoped<DateTimeSetterInterceptor>();
		services.AddScoped<PublishDomainEventsInterceptor>();
		
		return services;
	}

	private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		var authenticationSettings = configuration.Map<AuthenticationSettings>();
		
		services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					RequireAudience = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidateLifetime = true,
					ValidIssuer = authenticationSettings.Token.Issuer,
					ValidAudience = authenticationSettings.Token.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.Token.Secret))
				};
			})
			.AddGoogle(options =>
			{
				options.ClientId = authenticationSettings.Google.ClientId;
				options.ClientSecret = authenticationSettings.Google.ClientSecret;
			});

		services.AddSingleton<ITokenProvider, TokenProvider>();
		services.AddScoped<IGoogleAuthentication, GoogleAuthentication>();
		
		return services;
	}
	
	private static IServiceCollection AddRepositories(this IServiceCollection services)
	{
		services.AddScoped<IContentReadOnlyRepository, ContentReadOnlyRepository>();
		services.AddScoped<IContentRepository, ContentRepository>();
		services.AddScoped<IMediaReadOnlyRepository, MediaReadOnlyRepository>();
		services.AddScoped<IMediaRepository, MediaRepository>();
		services.AddScoped<IPersonReadOnlyRepository, PersonReadOnlyRepository>();
		services.AddScoped<IPersonRepository, PersonRepository>();
		services.AddScoped<IPlatformReadOnlyRepository, PlatformReadOnlyRepository>();
		services.AddScoped<IPlatformRepository, PlatformRepository>();
		services.AddScoped<IUserReadOnlyRepository, UserReadOnlyRepository>();
		services.AddScoped<IUserRepository, UserRepository>();

		return services;
	}

	private static NpgsqlDataSource GetNpgsqlDataSource(string? connectionString)
	{
		var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

		dataSourceBuilder.MapEnum<Role>();

		return dataSourceBuilder.Build();
	}
}