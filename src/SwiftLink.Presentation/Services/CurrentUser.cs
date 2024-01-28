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
            if (token is not null && Guid.TryParse(token, out Guid _token))
                return _token;
            return null;
        }
    }
}
