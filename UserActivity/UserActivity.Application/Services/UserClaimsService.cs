using Shared.Interfaces;
using Shared.Objects.Identity;

namespace UserActivity.Application.Services;

public class UserClaimsService : IUserClaimsService
{
    private IdentityContext? _userContext;

    public IdentityContext? UserContext => _userContext;

    public void SetUserClaims(IdentityContext? userContext)
    {
        _userContext = userContext;
    }
}