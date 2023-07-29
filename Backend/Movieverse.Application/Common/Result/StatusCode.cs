namespace Movieverse.Application.Common.Result;

public enum StatusCode
{
	NotFound = 404,
	Invalid = 400,
	AlreadyExists = 409,
	Unauthorized = 401,
	Forbidden = 403,
	InternalError = 500,
	NotImplemented = 501
}