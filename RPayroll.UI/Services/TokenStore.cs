using Microsoft.AspNetCore.Http;

namespace RPayroll.UI.Services;

public class TokenStore
{
    private const string TokenKey = "RPayroll.Token";
    private const string UserIdKey = "RPayroll.UserId";
    private const string UsernameKey = "RPayroll.Username";
    private const string RoleKey = "RPayroll.Role";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenStore(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ISession? Session => _httpContextAccessor.HttpContext?.Session;

    public string? Token => Session?.GetString(TokenKey);

    public int? UserId => int.TryParse(Session?.GetString(UserIdKey), out var id) ? id : null;

    public string? Username => Session?.GetString(UsernameKey);

    public string? Role => Session?.GetString(RoleKey);

    public void SetToken(string token, int userId, string username, string role)
    {
        if (Session == null)
        {
            return;
        }

        Session.SetString(TokenKey, token);
        Session.SetString(UserIdKey, userId.ToString());
        Session.SetString(UsernameKey, username);
        Session.SetString(RoleKey, role);
    }

    public void Clear()
    {
        Session?.Remove(TokenKey);
        Session?.Remove(UserIdKey);
        Session?.Remove(UsernameKey);
        Session?.Remove(RoleKey);
    }
}
