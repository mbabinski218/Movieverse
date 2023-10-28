using MediatR;
using Movieverse.Contracts.Payments.PayPal.Responses;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Payments.PayPal.Requests;

public sealed record GetSubscriptionRequest : IRequest<Result<SubscriptionResponse>>;