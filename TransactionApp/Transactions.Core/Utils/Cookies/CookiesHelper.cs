using Microsoft.AspNetCore.Http;

namespace Transactions.Core.Utils.Cookies;

public class CookiesHelper : ICookiesHelper
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CookiesHelper(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public void RemoveCookie(string cookieLabel)
    {
        _contextAccessor.HttpContext?.Response.Cookies.Delete(cookieLabel);
    }

    public string? GetCookie(string cookieLabel)
    {
        string? token = null;
        _ = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(cookieLabel, out token);
        return token ?? null;
    }

    public void AddCookie(string label, string token)
    {
        _contextAccessor.HttpContext?.Response.Cookies.Append(label, token);
    }
}
