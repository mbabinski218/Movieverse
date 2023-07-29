namespace Movieverse.Application.Common.Result;

public interface IResult
{
	bool IsSuccessful { get; }
}

public readonly struct Result : IResult
{
	private readonly Error? _error;
	public bool IsSuccessful { get; }

	public Result()
	{
		IsSuccessful = true;
	}
	
	private Result(Error error)
	{
		IsSuccessful = false;
		_error = error;
	}
	
	public static implicit operator Result(Error error) => new(error);
	
	public static Result Ok() => new();
	
	public static Result Error(Error error) => new(error);
	
	public TResult Match<TResult>(Func<TResult> success, Func<Error, TResult> error) =>
		IsSuccessful ? success() : error(_error!);
}

public readonly struct Result<TSuccess> : IResult
{
	private readonly TSuccess? _success;
	private readonly Error? _error;
	public bool IsSuccessful { get; }
	
	private Result(TSuccess value)
	{
		IsSuccessful = true;
		_success = value;
	}
	
	private Result(Error error)
	{
		IsSuccessful = false;
		_error = error;
	}

	public static implicit operator Result<TSuccess>(TSuccess value) => new(value);
	
	public static implicit operator Result<TSuccess>(Error error) => new(error);
	
	public static Result<TSuccess> Ok(TSuccess value) => new(value);
	
	public static Result<TSuccess> Error(Error error) => new(error);
	
	public TResult Match<TResult>(Func<TSuccess, TResult> success, Func<Error, TResult> error) =>
		IsSuccessful ? success(_success!) : error(_error!);
}