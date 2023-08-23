using Microsoft.AspNetCore.Authorization;

namespace Movieverse.API.Common;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class PolicyAuthorize : AuthorizeAttribute
{
	public PolicyAuthorize(string policy)
	{
		Policy = policy;
	}
	
	public PolicyAuthorize(params string[] policies)
	{
		Policy = string.Join(",", policies);
	}
}