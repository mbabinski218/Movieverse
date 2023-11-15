using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Commands.Media;

public record AddReviewCommand(
	[FromRoute] Guid Id, 
	string Text
	) : IRequest<Result<ReviewDto>>;