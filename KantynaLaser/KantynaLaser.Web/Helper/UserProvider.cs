using System.Security.Claims;

namespace KantynaLaser.Web.Helper;

public interface IUserProvider
{
    string GetUserId();
}
public class UserProvider : IUserProvider
{
    private readonly IHttpContextAccessor _context;

    public UserProvider(IHttpContextAccessor context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public string GetUserId()
    {
        return _context.HttpContext.User.Claims
                   .First(i => i.Type == ClaimTypes.NameIdentifier).Value;
    }
}
