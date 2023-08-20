using Movieverse.Application.Interfaces;

namespace Movieverse.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
	private readonly AppDbContext _context;

	public UnitOfWork(AppDbContext context)
	{
		_context = context;
	}

	public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return await _context.SaveChangesAsync(cancellationToken) > 0;
	}

	public void Dispose()
	{
		_context.Dispose();
	}

	public async ValueTask DisposeAsync()
	{
		await _context.DisposeAsync();
	}
}