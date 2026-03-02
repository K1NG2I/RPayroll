namespace RPayroll.UI.Services;

public class TokenStore
{
    public string? Token { get; private set; }
    public int? UserId { get; private set; }
    public string? Username { get; private set; }
    public string? Role { get; private set; }

    public void SetToken(string token, int userId, string username, string role)
    {
        Token = token;
        UserId = userId;
        Username = username;
        Role = role;
    }

    public void Clear()
    {
        Token = null;
        UserId = null;
        Username = null;
        Role = null;
    }
}
