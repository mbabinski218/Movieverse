using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Movieverse.API.Common;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class PolicyAuthorizeAttribute : AuthorizeAttribute
{
	public PolicyAuthorizeAttribute(string policy)
	{
		Policy = policy;
		AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
	}
}