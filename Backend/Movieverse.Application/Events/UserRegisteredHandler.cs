using System.Web;
using Movieverse.Application.Interfaces;
using Movieverse.Domain.DomainEvents;
using MediatR;

namespace Movieverse.Application.Events;

public sealed class UserRegisteredHandler : INotificationHandler<UserRegistered>
{
	private readonly IEmailServiceProvider _emailServiceProvider;
	private readonly IHttpService _httpService;

	public UserRegisteredHandler(IEmailServiceProvider emailServiceProvider, IHttpService httpService)
	{
		_emailServiceProvider = emailServiceProvider;
		_httpService = httpService;
	}

	public async Task Handle(UserRegistered notification, CancellationToken cancellationToken)
	{
		var url = _httpService.Uri?.GetLeftPart(UriPartial.Authority);
		var id = notification.UserId.ToString();
		var encodedToken = HttpUtility.UrlEncode(notification.Token);
		var link = $"{url}/api/user/confirm-email?Id={id}&Token={encodedToken}";
		
		await _emailServiceProvider.SendEmailConfirmationAsync(notification.Email, link, cancellationToken);
	}
}