using MediatR;
using Movieverse.Contracts.Queries;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Test;

public sealed class TestQueryHandler : IRequestHandler<TestQuery, Result>
{
	public async Task<Result> Handle(TestQuery request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}