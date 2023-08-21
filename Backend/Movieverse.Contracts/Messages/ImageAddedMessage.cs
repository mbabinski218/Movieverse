using Microsoft.AspNetCore.Http;
using Movieverse.Contracts.Common;

namespace Movieverse.Contracts.Messages;

public sealed record ImageAddedMessage( //TODO To remove
	string ImageName,
	IFormFile Image
    ) : IMessage;