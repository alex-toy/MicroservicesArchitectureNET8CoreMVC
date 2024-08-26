namespace Transactions.Core.Utils.Cookies
{
    public interface ICookiesHelper
    {
        void AddCookie(string label, string token);
        string? GetCookie(string cookieLabel);
        void RemoveCookie(string cookieLabel);
    }
}