using Movieverse.Application.Interfaces;

namespace Movieverse.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
	private readonly AppDbContext _context;

	public UnitOfWork(AppDbContext context)
	{
		_context = context;
	}

	public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return await _context.SaveChangesAsync(cancellationToken);
	}
	
	public void Dispose()
	{
		_context.Dispose();
		GC.SuppressFinalize(this);
	}
}