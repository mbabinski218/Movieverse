namespace Movieverse.Contracts.DataTransferObjects.User;

public record class TokensDto(
	string AccessToken, 
	string RefreshToken);