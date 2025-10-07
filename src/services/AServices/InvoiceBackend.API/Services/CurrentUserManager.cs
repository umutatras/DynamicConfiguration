using InvoiceBackend.Application.Interfaces;
using System.Security.Authentication;
using System.Security.Claims;

namespace InvoiceBackend.API.Services;

public class CurrentUserManager : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserManager(IHttpContextAccessor contextAccessor)
    {
        _httpContextAccessor = contextAccessor;
    }

    public int UserId => GetUserId();


    private int GetUserId()
    {
        var userIdClaim = _httpContextAccessor
            .HttpContext?
            .User?
            .FindFirstValue("uid");

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            throw new AuthenticationException("Invalid Token: User ID is missing or not a valid integer.");
        }

        return userId;
    }


}
