using Microsoft.AspNetCore.Mvc;
using Movieverse.Application.Common.Result;

namespace Movieverse.API.Common;

public static class ResultExtensions
{
	public static async Task<ActionResult> Then(this Task<Result> result, Func<ActionResult> success, Func<Error, ActionResult> error)
	{
		var response = await result;

		return response.Match(success, error);
	}
	
	public static async Task<ActionResult> Then<T>(this Task<Result<T>> result, Func<T, ActionResult> success, Func<Error, ActionResult> error)
	{
		var response = await result;

		return response.Match(success, error);
	}
}