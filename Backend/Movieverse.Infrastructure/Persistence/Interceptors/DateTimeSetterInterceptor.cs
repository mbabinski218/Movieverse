using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Movieverse.Domain.Common.Models;

namespace Movieverse.Infrastructure.Persistence.Interceptors;

public sealed class DateTimeSetterInterceptor : SaveChangesInterceptor
{
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

	private static void DateTimeSetter(DbContext? context)
	{
		context?.ChangeTracker
			.Entries()
			.Where(entry => entry.State is EntityState.Added or EntityState.Modified)
			.ToList()
			.ForEach(entry =>
			{
				if (entry.Entity is not IAggregateRoot entity) return;

				switch (entry.State)
				{
					case EntityState.Added:
						entity.CreatedAt = DateTimeOffset.UtcNow;
						break;
					case EntityState.Modified:
						entity.UpdatedAt = DateTimeOffset.UtcNow;
						break;
				}
			});
	}
}