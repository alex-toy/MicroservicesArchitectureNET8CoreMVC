using Microsoft.AspNetCore.Http;

namespace Transactions.Core.Utils;

public class CookieToken : ICookieToken
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CookieToken(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public void RemoveCookieToken()
    {
        _contextAccessor.HttpContext?.Response.Cookies.Delete(Constants.TokenCookie);
    }

    public string? GetTokenFromCookie()
    {
        string? token = null;
        bool? hasToken = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(Constants.TokenCookie, out token);
        return hasToken is true ? token : null;
    }

    public void SetCookieToken(string token)
    {
        _contextAccessor.HttpContext?.Response.Cookies.Append(Constants.TokenCookie, token);
    }
}