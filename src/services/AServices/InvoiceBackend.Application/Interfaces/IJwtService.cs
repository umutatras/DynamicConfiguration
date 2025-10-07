using InvoiceBackend.Application.Models.Jwt;

namespace InvoiceBackend.Application.Interfaces;

public interface IJwtService
{
    JwtGenerateTokenResponse GenerateToken(JwtGenerateTokenRequest request);
    bool ValidateToken(string token);

    int GetUserIdFromJwt(string token);
}
