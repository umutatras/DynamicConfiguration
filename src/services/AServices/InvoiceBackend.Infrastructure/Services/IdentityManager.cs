using FluentValidation;
using FluentValidation.Results;
using InvoiceBackend.Application.Interfaces;
using InvoiceBackend.Application.Models.Identity;
using InvoiceBackend.Application.Models.Jwt;
using InvoiceBackend.Domain.Entities;
using InvoiceBackend.Domain.Settings;
using InvoiceBackend.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Authentication;

namespace InvoiceBackend.Infrastructure.Services;

public class IdentityManager : IIdentityService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public IdentityManager(UserManager<AppUser> userManager, IJwtService jwtService, IOptions<JwtSettings> jwtSettings, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }
    // Kullanıcının kimliğini doğrular.
    public async Task<bool> AuthenticateAsync(IdentityAuthenticateRequest request)
    {
        // Kullanıcıyı e-posta adresine göre bul.
        var user = await _userManager.FindByEmailAsync(request.Email);

        // Kullanıcı bulunamazsa false döndür.
        if (user is null) return false;

        // Kullanıcının parolasını kontrol et ve sonucu döndür.
        return await _userManager.CheckPasswordAsync(user, request.Password);
    }

    // E-posta adresinin veritabanında olup olmadığını kontrol eder.
    public Task<bool> CheckEmailExistsAsync(string email)
    {
        return _userManager
        .Users
        .AnyAsync(x => x.Email == email);
    }

    public Task<bool> CheckIfEmailVerified(string email)
    {
        return _userManager
        .Users
        .AnyAsync(x => x.Email == email && x.EmailConfirmed);
    }

    public async Task<bool> CheckSecurityStampAsync(int userId, string securityStamp)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        return string.Equals(securityStamp, user.SecurityStamp);
    }



    // Kullanıcının giriş yapmasını sağlar.
    public async Task<IdentityLoginResponse> LoginAsync(IdentityLoginRequest request)
    {
        // Kullanıcıyı e-posta adresine göre bul.
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            throw new AuthenticationException("Invalid email or password.");

        // Kullanıcının rollerini al.
        var roles = await _userManager.GetRolesAsync(user);

        // user.Id GUID ise int'e çevirmeye çalış
        if (!int.TryParse(user.Id.ToString(), out int userId))
            throw new Exception("User ID is not a valid integer.");

        // JWT oluşturma isteği oluştur.
        var jwtRequest = new JwtGenerateTokenRequest(userId, user.Email, roles);

        // JWT oluştur.
        var jwtResponse = _jwtService.GenerateToken(jwtRequest);

        var refreshToken = CreateRefreshToken(user);

        // Giriş yanıtını döndür.
        return new IdentityLoginResponse(jwtResponse.Token, jwtResponse.ExpiresAt, refreshToken.Token, refreshToken.Expires);
    }


    public async Task<IdentityRefreshTokenResponse> RefreshTokenAsync(IdentityRefreshTokenRequest request)
    {
        var userId = _jwtService.GetUserIdFromJwt(request.AccessToken);

        // Kullanıcıyı ID'sine göre bul.
        var user = await _userManager.FindByIdAsync(userId.ToString());

        // _collection.FindOne

        // Kullanıcının rollerini al.
        var roles = await _userManager.GetRolesAsync(user);

        // JWT oluşturma isteği oluştur.
        var jwtRequest = new JwtGenerateTokenRequest(user.Id, user.Email, roles);

        // JWT oluştur.
        var jwtResponse = _jwtService.GenerateToken(jwtRequest);

        // Giriş yanıtını döndür.
        return new IdentityRefreshTokenResponse(jwtResponse.Token, jwtResponse.ExpiresAt);
    }

    // Yeni bir kullanıcı kaydeder.
    public async Task<IdentityRegisterResponse> RegisterAsync(IdentityRegisterRequest request)
    {

        // Yeni bir kullanıcı nesnesi oluştur.
        var user = new AppUser
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedByUserId = null,
            CreatedOn = DateTimeOffset.UtcNow,
            EmailConfirmed = false,
        };

        // Kullanıcıyı veritabanına kaydet.
        var result = await _userManager.CreateAsync(user, request.Password);

        // Kayıt işlemi başarısız olursa hata fırlat.
        if (!result.Succeeded) CreateAndThrowValidationException(result.Errors);

        // E-posta onaylama jetonu oluştur.
        var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        // Kayıt yanıtını döndür.
        return new IdentityRegisterResponse(user.Id, user.Email, emailToken);
    }




    // Doğrulama hatası oluşturur ve fırlatır.
    private void CreateAndThrowValidationException(IEnumerable<IdentityError> errors)
    {
        // Hata mesajlarını ve özelliklerini içeren yeni bir doğrulama hatası oluştur.
        var errorMessages = errors
        .Select(x => new ValidationFailure(x.Code, x.Description))
        .ToArray();

        // Doğrulama hatasını fırlat.
        throw new ValidationException(errorMessages);
    }

    private RefreshToken CreateRefreshToken(AppUser user)
    {
        var refreshToken = new RefreshToken
        {
            CreatedByUserId = user.Id,
            CreatedOn = DateTimeOffset.UtcNow,
            AppUserId = user.Id,
            Token = Ulid.NewUlid().ToString(), // Rastegele token oluştur.
            Expires = DateTime.UtcNow.Add(_jwtSettings.RefreshTokenExpiration), // Refresh tokenın süresini belirler.
            SecurityStamp = user.SecurityStamp, // Kullanıcının güvenlik damgasını kullanır.
        };


        _unitOfWork.GetRepository<RefreshToken>().Add(refreshToken);

        _unitOfWork.SaveChanges();

        return refreshToken;
    }
}