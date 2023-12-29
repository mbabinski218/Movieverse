using Movieverse.Contracts.Common;

namespace Movieverse.Contracts.Messages;

public sealed record UserRegisteredMessage(
	string Email, 
	string ConfirmationLink
    ) : IMessage;