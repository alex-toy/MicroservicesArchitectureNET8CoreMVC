namespace Transactions.Web.Utils;

public static class Constants
{
    public static string BonusAPIBase { get; set; }
    public static string TransactionAPIBase { get; set; }
    public static string ShoppingCartAPIBase { get; set; }
    public enum ApiType
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}
