using System.Security.Claims;

namespace BarTicaret.WebUI.Services;

public interface IAuthService
{
    ClaimsPrincipal? Authenticate(string username, string password);
    bool VerifyPassword(string username, string password);
}
