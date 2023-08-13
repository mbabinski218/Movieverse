using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.Interfaces;

public interface IEmailServiceProvider
{
	Task<Result> SendAsync(string email, string subject, string text, CancellationToken cancellationToken);
	Task<Result> SendEmailConfirmationAsync(string email, string link, CancellationToken cancellationToken);
}