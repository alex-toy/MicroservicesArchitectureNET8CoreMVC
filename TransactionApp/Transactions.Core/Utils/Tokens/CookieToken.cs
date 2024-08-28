using Transactions.Core.Utils.Cookies;

namespace Transactions.Core.Utils.Tokens;

public class CookieToken : ICookieToken
{
    private readonly ICookiesHelper _cookieHelper;

    public CookieToken(ICookiesHelper cookieHelper)
    {
        _cookieHelper = cookieHelper;
    }

    public void RemoveCookieToken()
    {
        _cookieHelper.RemoveCookie(Constants.TokenCookie);
    }

    public string? GetTokenFromCookie()
    {
        return _cookieHelper.GetCookie(Constants.TokenCookie);
    }

    public void SetCookieToken(string token)
    {
        _cookieHelper.AddCookie(Constants.TokenCookie, token);
    }
}