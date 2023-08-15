using MassTransit;
using Movieverse.Contracts.Messages;

namespace Movieverse.Consumer.Consumers;

public sealed class ImageAddedConsumer : IConsumer<ImageAddedMessage>
{
	public ImageAddedConsumer()
	{
		
	}

	public Task Consume(ConsumeContext<ImageAddedMessage> context)
	{
		throw new NotImplementedException();
	}
}