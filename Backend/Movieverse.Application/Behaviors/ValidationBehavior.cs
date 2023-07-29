using FluentValidation;
using MediatR;

namespace Movieverse.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : notnull
	where TResponse : notnull
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
	{
		_validators = validators;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		if (!_validators.Any())
		{
			return await next();
		}

		var validationResults = await Task.WhenAll
		(
			_validators.Select(v => v.ValidateAsync(request, cancellationToken))
		);

		var errors = validationResults
			.Where(result => !result.IsValid)
			.SelectMany(result => result.Errors)
			.ToList();

		if (errors.Any())
		{
			throw new ValidationException(errors);
		}

		return await next();
	}
}