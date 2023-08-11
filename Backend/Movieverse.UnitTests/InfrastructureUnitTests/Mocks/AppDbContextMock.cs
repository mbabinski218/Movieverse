using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Infrastructure.Persistence;
using Movieverse.Infrastructure.Persistence.Interceptors;
using NSubstitute;

namespace Movieverse.UnitTests.InfrastructureUnitTests.Mocks;

public static class AppDbContextMock
{
	public static AppDbContext Get()
	{
		var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("Movieverse").Options;
		var publishDomainEventsInterceptor = new PublishDomainEventsInterceptor(Substitute.For<IPublisher>(), Substitute.For<ILogger<PublishDomainEventsInterceptor>>());
		var dateTimeSetterInterceptor = new DateTimeSetterInterceptor(Substitute.For<ILogger<DateTimeSetterInterceptor>>());
		var logger = Substitute.For<ILogger<AppDbContext>>();
		
		return new AppDbContext(options, publishDomainEventsInterceptor, dateTimeSetterInterceptor, logger);
	}
}