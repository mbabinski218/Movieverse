

using Movieverse.Domain.Common.Result;

namespace Movieverse.Consumer.Interfaces;

public interface IEmailServiceProvider
{
	Task<Result> SendAsync(string email, string subject, string text, CancellationToken cancellationToken = default);
	Task<Result> SendEmailConfirmationAsync(string email, string link, CancellationToken cancellationToken = default);
}