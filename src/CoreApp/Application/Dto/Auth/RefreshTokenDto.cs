namespace CoreApp.Application.Dto.Auth;

public record RefreshTokenDto(
    string AccessToken,
    string RefreshToken
);
