using SwiftLink.Application.Common.Interfaces;

namespace SwiftLink.Presentation.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid? Token
    {
        get
        {
            var token = _httpContextAccessor.HttpContext?.Request?.Headers["Token"];
            return token is not null && Guid.TryParse(token, out var _token) ? _token : null;
        }
    }
}
