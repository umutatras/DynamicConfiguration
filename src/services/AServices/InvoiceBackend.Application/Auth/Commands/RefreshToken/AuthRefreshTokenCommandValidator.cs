using FluentValidation;
using InvoiceBackend.Application.Interfaces;

namespace InvoiceBackend.Application.Auth.RefreshToken;

public class AuthRefreshTokenCommandValidator : AbstractValidator<AuthRefreshTokenCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly IIdentityService _identityService;

    public AuthRefreshTokenCommandValidator(IUnitOfWork uow, IJwtService jwtService, IIdentityService identityService)
    {
        _unitOfWork = uow;
        _jwtService = jwtService;
        _identityService = identityService;
        RuleFor(x => x.AccessToken)
            .NotEmpty()
            .WithMessage("AccessToken is required")
            .MinimumLength(50)
            .MaximumLength(2000)
            .WithMessage("AccessToken must be between 50 and 2000 characters");

        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("RefreshToken is required")
            .MinimumLength(10)
            .MaximumLength(200)
            .WithMessage("RefreshToken must be between 10 and 200 characters");

        RuleFor(x => x.RefreshToken)
            .MustAsync((command, refreshToken, cancellationToken) => IsRefreshTokenValidAsync(command.AccessToken, refreshToken, cancellationToken))
            .WithMessage("RefreshToken is invalid");
    }

    private async Task<bool> IsRefreshTokenValidAsync(string accessToken, string refreshToken, CancellationToken cancellationToken)
    {
        var userId = _jwtService.GetUserIdFromJwt(accessToken);

        var refreshTokenEntity = _unitOfWork.GetRepository<InvoiceBackend.Domain.Entities.RefreshToken>().Query().FirstOrDefault(x => x.AppUserId == userId && x.Token == refreshToken);

        if (refreshTokenEntity is null || refreshTokenEntity.Expires < DateTime.UtcNow)
        {
            return false;
        }
        if (!await _identityService.CheckSecurityStampAsync(userId, refreshTokenEntity.SecurityStamp))
            return false;

        return true;
    }

}