namespace Transactions.Core.Dtos;

public class ResponseDto<T>
{
    public T Result { get; set; }
    public bool IsSuccess { get; set; } = true;
    public string ErrorMessage { get; set; } = string.Empty;
}
