using MediatR;
using Movieverse.Contracts.Payments.PayPal.Responses;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Payments.PayPal.Requests;

public sealed record PlanRequest(
	string PayPalAccessToken
	) : IRequest<Result<PlanResponse>>;