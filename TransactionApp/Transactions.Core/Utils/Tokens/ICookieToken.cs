namespace Transactions.Core.Utils.Tokens;

public interface ICookieToken
{
    void SetCookieToken(string token);
    string? GetTokenFromCookie();
    void RemoveCookieToken();
}
