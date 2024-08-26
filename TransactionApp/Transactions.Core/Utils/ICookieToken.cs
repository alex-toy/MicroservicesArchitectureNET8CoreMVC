namespace Transactions.Core.Utils;

public interface ICookieToken
{
    void SetCookieToken(string token);
    string? GetTokenFromCookie();
    void RemoveCookieToken();
}
