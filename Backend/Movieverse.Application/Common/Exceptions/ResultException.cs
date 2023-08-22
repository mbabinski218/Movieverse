using System.Diagnostics.CodeAnalysis;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.Common.Exceptions;

public sealed class ResultException : Exception
{
	public ResultException()
    {
    	
    }
	
	public ResultException(string msg) : base(msg)
	{
		
	}
	
	public static void ThrowIfUnsuccessful(IResult result, string? msg = null)
	{
		if (result.IsUnsuccessful)
		{
			Throw(msg);
		}
	}
	
	public static void ThrowIfUnsuccessful(Result result)
	{
		if (result.IsSuccessful)
		{
			return;
		}
		
		if (result.Error.Messages.Length == 0)
		{
			Throw();
		}
		Throw(result.Error.Messages[0]);
	}
    
	public static void ThrowIfUnsuccessful<T>(Result<T> result)
	{
		if (result.IsSuccessful)
		{
			return;
		}
		
		if (result.Error.Messages.Length == 0)
		{
			Throw();
		}
		Throw(result.Error.Messages[0]);
	}
	
	[DoesNotReturn]
	public static void Throw(string? msg = null) => throw new ResultException(msg ?? string.Empty);
}