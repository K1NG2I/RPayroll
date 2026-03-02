namespace RPayroll.UI.Services;

public class TokenStore
{
    public string? Token { get; private set; }

    public void SetToken(string token)
    {
        Token = token;
    }

    public void Clear()
    {
        Token = null;
    }
}
