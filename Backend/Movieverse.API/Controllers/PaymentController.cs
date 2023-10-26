using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movieverse.API.Common;
using Movieverse.API.Common.Extensions;
using Movieverse.Application.Authorization;
using Movieverse.Contracts.Payments.PayPal.Requests;
using Movieverse.Contracts.Payments.PayPal.Responses;

namespace Movieverse.API.Controllers;

public sealed class PaymentController : ApiController
{
	public PaymentController(IMediator mediator) : base(mediator)
	{
	}
	
	[PolicyAuthorize(Policies.atLeastUser)]
	[OutputCache(NoStore = true)]
	[HttpPost("paypal/authorization")]
	public async Task<ActionResult<AuthorizationResponse>> PayPalAuthorization(CancellationToken cancellationToken) =>
		await mediator.Send(new AuthorizationRequest(), cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[PolicyAuthorize(Policies.atLeastUser)]
	[OutputCache(NoStore = true)]
	[HttpGet("paypal/plan")]
	public async Task<ActionResult<PlanResponse>> PayPalGetPlan([FromQuery] PlanRequest request, CancellationToken cancellationToken) =>
		await mediator.Send(request, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	// [PolicyAuthorize(Policies.atLeastUser)]
	// [OutputCache(NoStore = true)]
	// [HttpPost("paypal/subscription")]
	// public async Task<ActionResult<SubscriptionResponse>> PayPalCreateSubscription([FromQuery] SubscriptionRequest request, CancellationToken cancellationToken) =>
	// 	await mediator.Send(request, cancellationToken).Then(
	// 		Ok,
	// 		err => StatusCode(err.Code, err.Messages));
	//
	// [PolicyAuthorize(Policies.atLeastUser)]
	// [OutputCache(NoStore = true)]
	// [HttpPost("paypal/subscription/{id}/active")]
	// public async Task<ActionResult<AuthorizationResponse>> PayPalActiveSubscription([FromQuery] PlanRequest request, CancellationToken cancellationToken) =>
	// 	await mediator.Send(request, cancellationToken).Then(
	// 		Ok,
	// 		err => StatusCode(err.Code, err.Messages));
	//
	// [PolicyAuthorize(Policies.atLeastPro)]
	// [OutputCache(NoStore = true)]
	// [HttpPost("paypal/subscription/{id}/cancel")]
	// public async Task<ActionResult<AuthorizationResponse>> PayPalCancelSubscription([FromQuery] PlanRequest request, CancellationToken cancellationToken) =>
	// 	await mediator.Send(request, cancellationToken).Then(
	// 		Ok,
	// 		err => StatusCode(err.Code, err.Messages));
}