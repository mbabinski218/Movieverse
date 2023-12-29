using System.Security.Claims;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.Common.Types;
using Movieverse.Infrastructure.Authentication;
using Movieverse.Infrastructure.Persistence;
using Movieverse.Infrastructure.Repositories;
using Movieverse.UnitTests.InfrastructureUnitTests.Mocks;
using NSubstitute;
using NUnit.Framework;

namespace Movieverse.UnitTests.InfrastructureUnitTests;

[TestFixture]
public class UserRepositoryUnitTests
{
	private IUserRepository _userRepository = null!;
	private ILogger<UserRepository> _logger = null!;
	private UserManager<User> _userManager = null!;
	private RoleManager<IdentityUserRole> _roleManager = null!;
	private ITokenProvider _tokenProvider = null!;
	private IGoogleAuthentication _googleAuthentication = null!;
	private Context _dbContext = null!;

	[SetUp]
	public void SetUp()
	{
		_logger = Substitute.For<ILogger<UserRepository>>();
		_userManager = UserManagerMock.Get<User>();
		_roleManager = RoleManagerMock.Get<IdentityUserRole>();
		_tokenProvider = Substitute.For<ITokenProvider>();
		_googleAuthentication = Substitute.For<IGoogleAuthentication>();
		_dbContext = ContextMock.Get();
           
		_userRepository = new UserRepository(_logger, _dbContext, _userManager, _roleManager, _tokenProvider, _googleAuthentication);
	}
	
	[TearDown]
	public void TearDown()
	{
		_dbContext.Database.EnsureDeleted();
	}

	[Test]
	public async Task FindByIdAsync_WhenUserExists_ReturnsUser()
	{
		// Arrange
		var userId = Guid.NewGuid();
		var expectedUser = User.Create(userId, "test@test.com", "Test", "Test", "Test", 20);
		_dbContext.Users.FindAsync(Arg.Any<object[]>(), CancellationToken.None).Returns(expectedUser);

		// Act
		var result = await _userRepository.FindByIdAsync(userId);

		// Assert
		Assert.AreEqual(expectedUser, result.Value);
		Assert.IsTrue(result.IsSuccessful);
	}
	
	[Test]
	public async Task FindByIdAsync_WhenUserDoesNotExist_ReturnsNotFoundError()
	{
		// Arrange
		var userId = Guid.NewGuid();
		_dbContext.Users.FindAsync(Arg.Is<object[]>(x => x.Contains(userId)), CancellationToken.None)
			.Returns(ValueTask.FromResult<User?>(null));

		// Act
		var result = await _userRepository.FindByIdAsync(userId);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
		Assert.AreEqual(new[] { UserResources.UserDoesNotExist }, result.Error.Messages);
	}
	
	[Test]
	public void FindByIdAsync_WhenDbContextThrowsException_ThrowsSameException()
	{
		// Arrange
		var userId = Guid.NewGuid();
		_dbContext.Users.FindAsync(Arg.Any<object[]>(), CancellationToken.None)
			.Returns(_ => ValueTask.FromException<User?>(new Exception()));

		// Act & Assert
		Assert.ThrowsAsync<Exception>(() => _userRepository.FindByIdAsync(userId));
	}
	
	[Test]
	public async Task FindByEmailAsync_WhenUserExists_ReturnsUser()
	{
		// Arrange
		const string email = "test@test.com";
		var expectedUser = User.Create(Guid.NewGuid(), email, "Test", "Test", "Test", 20);
		_userManager.FindByEmailAsync(email).Returns(expectedUser);

		// Act
		var result = await _userRepository.FindByEmailAsync(email);

		// Assert
		Assert.AreEqual(expectedUser, result.Value);
		Assert.IsTrue(result.IsSuccessful);
	}
	
	[Test]
	public async Task FindByEmailAsync_WhenUserDoesNotExist_ReturnsNotFoundError()
	{
		// Arrange
		const string email = "test@test.com";
		_userManager.FindByEmailAsync(email).Returns(Task.FromResult<User?>(null));

		// Act
		var result = await _userRepository.FindByEmailAsync(email);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
		Assert.AreEqual(new[] { UserResources.UserDoesNotExist }, result.Error.Messages);
	}
	
	[Test]
	public void FindByEmailAsync_WhenDbContextThrowsException_ThrowsSameException()
	{
		// Arrange
		const string email = "test@test.com";
		_userManager.FindByEmailAsync(email).Returns(_ => Task.FromException<User?>(new Exception()));

		// Act & Assert
		Assert.ThrowsAsync<Exception>(() => _userRepository.FindByEmailAsync(email));
	}
	
	[Test]
	public async Task GenerateEmailConfirmationTokenAsync_WhenCalled_ReturnsToken()
	{
		// Arrange
		var user = User.Create(Guid.NewGuid(), "test@test.com", "Test", "Test", "Test", 20);
		const string expectedToken = "confirmationToken";
		_userManager.GenerateEmailConfirmationTokenAsync(user).Returns(expectedToken);

		// Act
		var result = await _userRepository.GenerateEmailConfirmationTokenAsync(user);

		// Assert
		Assert.AreEqual(expectedToken, result.Value);
		Assert.IsTrue(result.IsSuccessful);
	}
	
	[Test]
	public void GenerateEmailConfirmationTokenAsync_WhenExceptionOccurs_ThrowsSameException()
	{
		// Arrange
		var user = User.Create(Guid.NewGuid(), "test@test.com", "Test", "Test", "Test", 20);
		_userManager.GenerateEmailConfirmationTokenAsync(user).Returns(_ => Task.FromException<string>(new Exception()));

		// Act & Assert
		Assert.ThrowsAsync<Exception>(() => _userRepository.GenerateEmailConfirmationTokenAsync(user));
	}
	
	[Test]
	public async Task RegisterAsync_SuccessfulRegistration_ReturnsEmailConfirmationToken()
	{
		// Arrange
		var user = User.Create(Guid.NewGuid(), "test@test.com", "Test", "Test", "Test", 20);
		const string password = "password";
		const string expectedToken = "confirmationToken";
		
		_userManager.CreateAsync(Arg.Any<User>(), Arg.Any<string>())
			.Returns(Task.FromResult(IdentityResult.Success));
		
		_userManager.AddToRoleAsync(Arg.Any<User>(), Arg.Any<string>())
			.Returns(Task.FromResult(IdentityResult.Success));
		
		_userManager.AddClaimsAsync(Arg.Any<User>(), Arg.Any<IEnumerable<Claim>>())
			.Returns(Task.FromResult(IdentityResult.Success));
		
		_userManager.GenerateEmailConfirmationTokenAsync(user).Returns(expectedToken);

		// Act
		var result = await _userRepository.RegisterAsync(user, password);

		// Assert
		Assert.AreEqual(expectedToken, result.Value);
		Assert.IsTrue(result.IsSuccessful);
	}
	
	[Test]
	public async Task RegisterAsync_UserCreationFails_ReturnsError()
	{
		// Arrange
		var user = User.Create(Guid.NewGuid(), "test@test.com", "Test", "Test", "Test", 20);
		const string password = "password";
		
		_userManager.CreateAsync(user, password).Returns(IdentityResult.Failed());

		// Act
		var result = await _userRepository.RegisterAsync(user, password);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
	}
	
	[Test]
	public async Task RegisterAsync_AddToRoleFails_ReturnsError()
	{
		// Arrange
		var user = User.Create(Guid.NewGuid(), "test@test.com", "Test", "Test", "Test", 20);
		const string password = "password";
		
		_userManager.CreateAsync(Arg.Any<User>(), Arg.Any<string>())
			.Returns(Task.FromResult(IdentityResult.Success));
		
		_userManager.AddToRoleAsync(user, UserRole.User.ToString())
			.Returns(IdentityResult.Failed());

		// Act
		var result = await _userRepository.RegisterAsync(user, password);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
		Assert.AreEqual(new[] { UserResources.FailedToRegisterUser }, result.Error.Messages);
	}
	
	[Test]
	public async Task RegisterAsync_AddClaimsFails_ReturnsError()
	{
		// Arrange
		var user = User.Create(Guid.NewGuid(), "test@test.com", "Test", "Test", "Test", 20);
		const string password = "password";

		_userManager.CreateAsync(Arg.Any<User>(), Arg.Any<string>())
			.Returns(Task.FromResult(IdentityResult.Success));
		
		_userManager.AddToRoleAsync(Arg.Any<User>(), Arg.Any<string>())
			.Returns(Task.FromResult(IdentityResult.Success));
		
		_userManager.AddClaimsAsync(user, Arg.Any<List<Claim>>())
			.Returns(IdentityResult.Failed());

		// Act
		var result = await _userRepository.RegisterAsync(user, password);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
		Assert.AreEqual(new[] { UserResources.FailedToRegisterUser }, result.Error.Messages);
	}
	
	[Test]
	public async Task ConfirmEmailAsync_SuccessfulConfirmation_ReturnsSuccess()
	{
		// Arrange
		var user = User.Create(Guid.NewGuid(), "test@test.com", "Test", "Test", "Test", 20);
		const string token = "confirmationToken";
		
		_userManager.ConfirmEmailAsync(user, token).Returns(IdentityResult.Success);

		// Act
		var result = await _userRepository.ConfirmEmailAsync(user, token);

		// Assert
		Assert.IsTrue(result.IsSuccessful);
	}
	
	[Test]
	public async Task ConfirmEmailAsync_FailedConfirmation_ReturnsError()
	{
		// Arrange
		var user = User.Create(Guid.NewGuid(), "test@test.com", "Test", "Test", "Test", 20);
		const string token = "confirmationToken";
		
		_userManager.ConfirmEmailAsync(user, token).Returns(IdentityResult.Failed());

		// Act
		var result = await _userRepository.ConfirmEmailAsync(user, token);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
		Assert.AreEqual(new[] { UserResources.FailedToRegisterUser }, result.Error.Messages);
	}
	
	[Test]
	public void ConfirmEmailAsync_WhenExceptionOccurs_ThrowsSameException()
	{
		// Arrange
		var user = User.Create(Guid.NewGuid(), "test@test.com", "Test", "Test", "Test", 20);
		const string token = "confirmationToken";

		_userManager.ConfirmEmailAsync(user, token)
			.Returns(Task.FromException<IdentityResult>(new Exception()));

		// Act & Assert
		Assert.ThrowsAsync<Exception>(() => _userRepository.ConfirmEmailAsync(user, token));
	}
	
	[Test]
	public async Task LoginAsync_SuccessfulLogin_ReturnsTokensDto()
	{
		// Arrange
		var user = User.Create(Guid.NewGuid(), "test@test.com", "Test", "Test", "Test", 20);
		const string password = "correctPassword";
		
		_userManager.IsEmailConfirmedAsync(user).Returns(Task.FromResult(true));
		
		_userManager.CheckPasswordAsync(user, password).Returns(Task.FromResult(true));
		
		MockGetClaims(user);
		
		_tokenProvider.GenerateAccessToken(Arg.Any<IEnumerable<Claim>>()).Returns("accessToken");
		_tokenProvider.GenerateRefreshToken().Returns("refreshToken");
		
		_userManager.SetAuthenticationTokenAsync(user, GrantType.Password.ToStringFast(), "RefreshToken", Arg.Any<string>())
			.Returns(Task.FromResult(IdentityResult.Success));

		// Act
		var result = await _userRepository.LoginAsync(user, password);

		// Assert
		Assert.IsTrue(result.IsSuccessful);
		Assert.IsNotNull(result.Value);
		Assert.IsInstanceOf<TokensDto>(result.Value);
	}
	
	[Test]
	public async Task LoginAsync_EmailNotConfirmed_ReturnsUnauthorizedError()
	{
		// Arrange
		var user = User.Create(Guid.NewGuid(), "test@test.com", "Test", "Test", "Test", 20);
		const string password = "correctPassword";
		
		_userManager.IsEmailConfirmedAsync(user).Returns(Task.FromResult(false));

		// Act
		var result = await _userRepository.LoginAsync(user, password);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
		Assert.AreEqual(new[] { UserResources.EmailIsNotConfirmed }, result.Error.Messages);
	}
	
	[Test]
	public async Task LoginAsync_InvalidPassword_ReturnsUnauthorizedError()
	{
		// Arrange
		var user = User.Create(Guid.NewGuid(), "test@test.com", "Test", "Test", "Test", 20);
		const string password = "wrongPassword";
		
		_userManager.IsEmailConfirmedAsync(user).Returns(Task.FromResult(true));
		_userManager.CheckPasswordAsync(user, password).Returns(Task.FromResult(false));

		// Act
		var result = await _userRepository.LoginAsync(user, password);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
		Assert.AreEqual(new[] { UserResources.InvalidPasswordOrEmail }, result.Error.Messages);
	}
	
	[Test]
	public async Task LoginWithRefreshTokenAsync_ValidToken_ReturnsNewTokensDto()
	{
		// Arrange
		var user = User.Create(Guid.NewGuid(), "test@test.com", "Test", "Test", "Test", 20);
		const string refreshToken = "validRefreshToken";
		
		MockRefreshTokenSetup(user, refreshToken, true);

		// Act
		var result = await _userRepository.LoginWithRefreshTokenAsync(user, refreshToken);

		// Assert
		Assert.IsTrue(result.IsSuccessful);
		Assert.IsInstanceOf<TokensDto>(result.Value);
	}
	
	[Test]
	public async Task LoginWithRefreshTokenAsync_InvalidToken_ReturnsUnauthorizedError()
	{
		// Arrange
		var user = User.Create(Guid.NewGuid(), "test@test.com", "Test", "Test", "Test", 20);
		const string invalidRefreshToken = "invalidRefreshToken";
		MockRefreshTokenSetup(user, invalidRefreshToken, false);

		// Act
		var result = await _userRepository.LoginWithRefreshTokenAsync(user, invalidRefreshToken);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
		Assert.AreEqual(new[] { UserResources.FailedToLoginWithRefreshToken }, result.Error.Messages);
	}
	
	[Test]
	public async Task LoginWithRefreshTokenAsync_NoTokenFound_ReturnsUnauthorizedError()
	{
		// Arrange
		var user = User.Create(Guid.NewGuid(), "test@test.com", "Test", "Test", "Test", 20);
		const string refreshToken = "missingToken";
		_userManager.GetAuthenticationTokenAsync(user, Arg.Any<string>(), "RefreshToken")
			.Returns(Task.FromResult<string?>(null));

		// Act
		var result = await _userRepository.LoginWithRefreshTokenAsync(user, refreshToken);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
		Assert.AreEqual(new[] { UserResources.FailedToLoginWithRefreshToken }, result.Error.Messages);
	}

	[Test]
	public async Task LoginWithGoogleAsync_SuccessfulLogin_ReturnsTokensDto()
	{
		// Arrange
		const string email = "test@test.com";
		const string idToken = "validIdToken";
		var user = User.Create(Guid.NewGuid(), email, "Test", "Test", "Test", 20);
		
		var googleUser = new GoogleUser
		{
			IsRegistered = true,
			Payload = new GoogleJsonWebSignature.Payload
			{
				Email = email,
				GivenName = "Test"
			}
		};
		_googleAuthentication.AuthenticateAsync(idToken)
			.Returns(Task.FromResult(Result<GoogleUser>.Ok(googleUser)));
		
		_userManager.FindByEmailAsync(email).Returns(Task.FromResult<User?>(user));

		// Act
		var result = await _userRepository.LoginWithGoogleAsync(idToken);

		// Assert
		Assert.IsTrue(result.IsSuccessful);
		Assert.IsInstanceOf<TokensDto>(result.Value);
	}
	
	[Test]
	public async Task LoginWithGoogleAsync_GoogleAuthenticationFails_ReturnsError()
	{
		// Arrange
		const string idToken = "invalidIdToken";
		_googleAuthentication.AuthenticateAsync(idToken).Returns(Result<GoogleUser>.Fail(Error.Invalid()));

		// Act
		var result = await _userRepository.LoginWithGoogleAsync(idToken);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
	}
	
	[Test]
	public async Task LoginWithGoogleAsync_UserNotFound_ReturnsError()
	{
		// Arrange
		const string idToken = "idTokenForExistingUser";
		const string email = "test@test.com";
		
		var googleUser = new GoogleUser
		{
			IsRegistered = true,
			Payload = new GoogleJsonWebSignature.Payload
			{
				Email = email,
				GivenName = "Test"
			}
		};
		_googleAuthentication.AuthenticateAsync(idToken)
			.Returns(Task.FromResult(Result<GoogleUser>.Ok(googleUser)));
		
		_userManager.FindByEmailAsync(Arg.Any<string>()).Returns(Task.FromResult<User?>(null));

		// Act
		var result = await _userRepository.LoginWithGoogleAsync(idToken);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
	}
    
    [Test]
    public async Task ChangePasswordAsync_SuccessfulChange_ReturnsSuccess()
    {
        // Arrange
        var user = User.Create(Guid.NewGuid(), "test@test.com", "Test", "Test", "Test", 20);
        const string currentPassword = "currentPassword";
        const string newPassword = "newPassword";

        user.PasswordHash = currentPassword;
        _userManager.ChangePasswordAsync(user, currentPassword, newPassword).Returns(IdentityResult.Success);
    
        // Act
        var result = await _userRepository.ChangePasswordAsync(user, currentPassword, newPassword);
    
        // Assert
        Assert.IsTrue(result.IsSuccessful);
    }
    
    [Test]
    public async Task ChangePasswordAsync_UserHasExternalLogin_ReturnsError()
    {
	    // Arrange
	    var user = User.Create(Guid.NewGuid(), "test@test.com", "Test", "Test", "Test", 20);
	    const string currentPassword = "currentPassword";
	    const string newPassword = "newPassword";

	    // Act
	    var result = await _userRepository.ChangePasswordAsync(user, currentPassword, newPassword);

	    // Assert
	    Assert.IsTrue(result.IsUnsuccessful);
	    Assert.AreEqual(new[] { UserResources.CanNotChangeExternalLoginPassword }, result.Error.Messages);
    }
    
    [Test]
    public async Task ChangePasswordAsync_ChangeFails_ReturnsError()
    {
	    // Arrange
	    var user = User.Create(Guid.NewGuid(), "test@test.com", "Test", "Test", "Test", 20);
	    const string currentPassword = "currentPassword";
	    const string newPassword = "newPassword";

	    user.PasswordHash = currentPassword;
	    _userManager.ChangePasswordAsync(user, currentPassword, newPassword).Returns(IdentityResult.Failed());

	    // Act
	    var result = await _userRepository.ChangePasswordAsync(user, currentPassword, newPassword);

	    // Assert
	    Assert.IsTrue(result.IsUnsuccessful);
	    Assert.AreEqual(new[] { UserResources.FailedToChangePassword }, result.Error.Messages);
    }
    
	//  Helper methods
	private void MockGetClaims(User user)
	{
		var userClaims = new List<Claim> { new(ClaimNames.id, user.Id.ToString()) };
		var roleClaimsMap = new Dictionary<string, List<Claim>>
		{
			{ UserRole.User.ToString(), new List<Claim> { new(ClaimNames.id, user.Id.ToString()) } }
		};
		
		_userManager.GetClaimsAsync(user).Returns(Task.FromResult(userClaims as IList<Claim>));

		var userRoles = roleClaimsMap.Keys.ToList();
		_userManager.GetRolesAsync(user).Returns(Task.FromResult(userRoles as IList<string>));

		foreach (var role in userRoles)
		{
			var roleObject = new IdentityUserRole(role);
			_roleManager.FindByNameAsync(role).Returns(Task.FromResult((IdentityUserRole?)roleObject));

			var roleSpecificClaims = roleClaimsMap[role];
			_roleManager.GetClaimsAsync(roleObject).Returns(Task.FromResult(roleSpecificClaims as IList<Claim>));
		}
	}
	
	private void MockRefreshTokenSetup(User user, string refreshToken, bool isValidToken)
	{
		if (isValidToken)
		{
			_userManager.GetAuthenticationTokenAsync(user, Arg.Any<string>(), "RefreshToken")
				.Returns(Task.FromResult((string?)refreshToken));
		}
		else
		{
			_userManager.GetAuthenticationTokenAsync(user, Arg.Any<string>(), "RefreshToken")
				.Returns(Task.FromResult<string?>(null));
		}
		
		_userManager.RemoveAuthenticationTokenAsync(user, Arg.Any<string>(), "RefreshToken")
			.Returns(Task.FromResult(IdentityResult.Success));
		
		_userManager.SetAuthenticationTokenAsync(user, Arg.Any<string>(), "RefreshToken", Arg.Any<string>())
			.Returns(Task.FromResult(IdentityResult.Success));
	}
}