using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Movieverse.UnitTests.InfrastructureUnitTests.Mocks;

public static class SignInManagerMock
{
	public static SignInManager<TUser> Get<TUser>()
		where TUser : class
	{
		var userManager = UserManagerMock.Get<TUser>();
		
		var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
		var userPrincipalFactory = Substitute.For<IUserClaimsPrincipalFactory<TUser>>();
		var options = Substitute.For<IOptions<IdentityOptions>>();
		var logger = Substitute.For<ILogger<SignInManager<TUser>>>();
		var schemes = Substitute.For<IAuthenticationSchemeProvider>();
		var confirmation = Substitute.For<IUserConfirmation<TUser>>();

		return Substitute.For<SignInManager<TUser>>(userManager, httpContextAccessor, userPrincipalFactory, options, logger, schemes, confirmation);
	}
}