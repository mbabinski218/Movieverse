namespace Movieverse.Application.Common.Result;

public sealed class Error
{
	public ushort Code { get; }
	public List<string> Messages { get; }
	
	public Error(ushort code)
	{
		Code = code;
		Messages = new List<string>();
	}
	
	public Error(ushort code, string message)
	{
		Code = code;
		Messages = new List<string> { message };
	}
	
	public Error(ushort code, List<string> messages)
	{
		Code = code;
		Messages = messages;
	}
	
	private Error(StatusCode code)
	{
		Code = (ushort)code;
		Messages = new List<string>();
	}
	
	private Error(StatusCode code, List<string> messages)
	{
		Code = (ushort)code;
		Messages = messages;
	}
	
	private Error(StatusCode code, string message)
	{
		Code = (ushort)code;
		Messages = new List<string> { message };
	}

	public static Error NotFound(string message) => new(StatusCode.NotFound, message);
	public static Error Invalid(string message) => new(StatusCode.Invalid, message);
	public static Error AlreadyExists(string message) => new(StatusCode.AlreadyExists, message);
	public static Error Unauthorized(string message) => new(StatusCode.Unauthorized, message);
	public static Error Forbidden(string message) => new(StatusCode.Forbidden, message);
	public static Error NotImplemented() => new(StatusCode.NotImplemented);
	public static Error InternalError(string message) => new(StatusCode.InternalError, message);
	public static Error ValidationError(List<string> message) => new(StatusCode.Invalid, message);
}