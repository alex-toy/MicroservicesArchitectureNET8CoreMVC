namespace Transactions.Web.Dtos;

public class ResponseDto<TResponse>
{
    public TResponse? Result { get; set; } = default;
    public bool IsSuccess { get; set; } = true;
    public string ErrorMessage { get; set; } = string.Empty;
}
