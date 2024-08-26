namespace Transactions.Core.Utils;

public interface ITokenProvider
{
    void SetToken(string token);
    string? GetToken();
    void ClearToken();
}
