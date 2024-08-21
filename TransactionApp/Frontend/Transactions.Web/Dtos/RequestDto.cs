using System.Data.SqlTypes;
using static Transactions.Web.Utils.Constants;

namespace Transactions.Web.Dtos;

public class RequestDto<TRequest>
{
    public ApiType ApiType { get; set; } = ApiType.GET;
    public string Url { get; set; }
    public TRequest? Data { get; set; }
    public string AccessToken { get; set; }
}
