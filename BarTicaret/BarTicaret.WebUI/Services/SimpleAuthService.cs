using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BarTicaret.WebUI.Services;

public class SimpleAuthService : IAuthService
{
    private readonly List<(string user, string pass, string role)> _users = new()
    {
        ("admin", "1234", "Admin"),
        ("Baran",   "135792468", "Admin"),
        ("Baran2",   "1357924689", "User")
    };

    public ClaimsPrincipal? Authenticate(string username, string password)
    {
        var u = _users.FirstOrDefault(x => x.user == username && x.pass == password);
        if (u == default) return null;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, u.role)
        };
        return new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
    }

    public bool VerifyPassword(string username, string password)
        => _users.Any(x => x.user == username && x.pass == password);
}
