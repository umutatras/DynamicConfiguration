using InvoiceBackend.Application.Models.Identity;

namespace InvoiceBackend.Application.Interfaces;

public interface IIdentityService
{
    Task<bool> AuthenticateAsync(IdentityAuthenticateRequest request);
    Task<IdentityLoginResponse> LoginAsync(IdentityLoginRequest request);

    Task<bool> CheckEmailExistsAsync(string email);
    Task<IdentityRegisterResponse> RegisterAsync(IdentityRegisterRequest request);


    Task<bool> CheckSecurityStampAsync(int userId, string securityStamp);
    Task<IdentityRefreshTokenResponse> RefreshTokenAsync(IdentityRefreshTokenRequest request);
}
