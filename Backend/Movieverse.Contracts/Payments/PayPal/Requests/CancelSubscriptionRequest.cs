using MediatR;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Payments.PayPal.Requests;

public sealed record CancelSubscriptionRequest(
	string PayPalAccessToken
	) : IRequest<Result>;