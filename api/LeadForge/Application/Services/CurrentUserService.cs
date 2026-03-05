using System.Security.Authentication;
using System.Security.Claims;
using LeadForge.Application.Interfaces;

namespace LeadForge.Application;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes
            .NameIdentifier);

        if (userId is null)
            throw new InvalidCredentialException("Can not restore user session from claim.");

        return Guid.Parse(userId);
    }

}