using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Movieverse.Application.Common.Result;
using Movieverse.Application.Common.Settings;
using Movieverse.Application.Interfaces;

namespace Movieverse.Application.Services;

public sealed class EmailServiceProvider : IEmailServiceProvider
{
	private readonly ILogger<EmailServiceProvider> _logger;
	private readonly EmailServiceSettings _emailServiceSettings;

	public EmailServiceProvider(ILogger<EmailServiceProvider> logger, IOptions<EmailServiceSettings> emailServiceSettings)
	{
		_logger = logger;
		_emailServiceSettings = emailServiceSettings.Value;
	}

	public async Task<Result> SendAsync(string email, string subject, string text, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Sending email to {email} with subject: {subject}.", email, subject);

		try
		{
			var message = new MimeMessage
			{
				From = { MailboxAddress.Parse(_emailServiceSettings.UserName) },
				To = { MailboxAddress.Parse(email) },
				Subject = subject,
				Body = new TextPart(TextFormat.Text) { Text = text }
			};

			using var smtp = new SmtpClient();
			await smtp.ConnectAsync(_emailServiceSettings.Host, _emailServiceSettings.Port, SecureSocketOptions.StartTls, cancellationToken);
			await smtp.AuthenticateAsync(_emailServiceSettings.UserName, _emailServiceSettings.Password, cancellationToken);
			await smtp.SendAsync(message, cancellationToken);
			await smtp.DisconnectAsync(true, cancellationToken);
		}
		catch (Exception ex)
		{
			_logger.LogError("Exception when sending email: {ex}", ex.Message);
			return Error.InternalError(ex.Message);
		}
		
		return Result.Ok();
	}

	public async Task<Result> SendEmailConfirmationAsync(string email, string link, CancellationToken cancellationToken)
	{
		const string subject = "Welcome to Movieverse!";
		var text = $"""
		            Welcome to Movieverse!
		            You have successfully registered your account.
		            Verify your email address by clicking the link below:
		            {link}

		            See you soon!
		            """;
		
		return await SendAsync(email, subject, text, cancellationToken);
	}
}