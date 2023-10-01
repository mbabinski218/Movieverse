namespace Movieverse.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
	Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}