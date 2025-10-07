using InvoiceBackend.Application.Models.General;
using MediatR;


namespace InvoiceBackend.Application.Auth.RefreshToken;

public sealed class AuthRefreshTokenCommand : IRequest<ResponseDto<AuthRefreshTokenResponse>>
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public AuthRefreshTokenCommand()
    {

    }
    public AuthRefreshTokenCommand(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}
