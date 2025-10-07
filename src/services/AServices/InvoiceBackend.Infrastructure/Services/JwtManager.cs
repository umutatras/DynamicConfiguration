using InvoiceBackend.Application.Interfaces;
using InvoiceBackend.Application.Models.Jwt;
using InvoiceBackend.Domain.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;

namespace InvoiceBackend.Infrastructure.Services;

public class JwtManager : IJwtService
{
    private readonly JwtSettings _jwtSettings;

    public JwtManager(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public JwtGenerateTokenResponse GenerateToken(JwtGenerateTokenRequest request)
    {
        var expirationInMinutes = _jwtSettings.AccessTokenExpiration;
        var expirationDate = DateTime.Now.AddMinutes(expirationInMinutes.Minutes);
        var claims = new List<Claim>
            {
                 new Claim("uid", request.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, request.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, _jwtSettings.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud, _jwtSettings.Audience),
                new Claim(JwtRegisteredClaimNames.Exp, expirationDate.ToFileTimeUtc().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToFileTimeUtc().ToString()),
                new Claim(ClaimTypes.Role,"Admin")
            }
        .Union(request.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var symmetricSecurtiyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var signingCredentials = new SigningCredentials(symmetricSecurtiyKey, SecurityAlgorithms.HmacSha256);
        var jwtSecurityToken = new JwtSecurityToken(issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expirationDate,
            signingCredentials: signingCredentials);

        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        return new JwtGenerateTokenResponse(token, expirationDate);
    }

    public int GetUserIdFromJwt(string token)
    {
        try
        {
            var claims = ParseClaimsFromJwt(token);
            var userIdClaim = claims.FirstOrDefault(c => c.Type == "uid")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new AuthenticationException("Invalid Token: User ID is missing or not a valid integer.");
            }

            return userId;
        }
        catch (Exception ex)
        {
            throw new AuthenticationException("Invalid token", ex);
        }
    }
    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
    public bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var secretKey = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }
}