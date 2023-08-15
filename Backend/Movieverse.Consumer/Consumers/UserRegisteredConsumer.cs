using MassTransit;
using Movieverse.Consumer.Interfaces;
using Movieverse.Contracts.Messages;

namespace Movieverse.Consumer.Consumers;

public sealed class UserRegisteredConsumer : IConsumer<UserRegisteredMessage>
{
	private readonly IEmailServiceProvider _emailServiceProvider;

	public UserRegisteredConsumer(IEmailServiceProvider emailServiceProvider)
	{
		_emailServiceProvider = emailServiceProvider;
	}

	public async Task Consume(ConsumeContext<UserRegisteredMessage> context)
	{
		await _emailServiceProvider.SendEmailConfirmationAsync(context.Message.Email, context.Message.ConfirmationLink);
	}
}