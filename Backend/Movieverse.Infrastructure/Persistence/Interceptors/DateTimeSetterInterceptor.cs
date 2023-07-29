using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Movieverse.Domain.Common.Models;

namespace Movieverse.Infrastructure.Persistence.Interceptors;

public sealed class DateTimeSetterInterceptor : SaveChangesInterceptor
{
	private readonly ILogger<DateTimeSetterInterceptor> _logger;

	public DateTimeSetterInterceptor(ILogger<DateTimeSetterInterceptor> logger)
	{
		_logger = logger;
	}

	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		DateTimeSetter(eventData.Context);
		return base.SavingChanges(eventData, result);
	}
	
	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new())
	{
		DateTimeSetter(eventData.Context);
		return await base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	private void DateTimeSetter(DbContext? context)
	{
		_logger.LogDebug("Setting date time...");
		
		context?.ChangeTracker
			.Entries()
			.Where(entry => entry.State is EntityState.Added or EntityState.Modified)
			.ToList()
			.ForEach(entry =>
			{
				if (entry.Entity is not AggregateRoot entity)
				{
					return;
				}

				if (entry.State is EntityState.Added)
				{
					entity.CreatedAt = DateTimeOffset.Now;
				}
				else
				{
					entity.UpdatedAt = DateTimeOffset.Now;
				}
			});
	}
}