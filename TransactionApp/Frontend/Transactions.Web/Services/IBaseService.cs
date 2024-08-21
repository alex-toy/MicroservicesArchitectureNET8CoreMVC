using Transactions.Web.Dtos;

namespace Transactions.Web.Services;

public interface IBaseService<T>
{
    Task<ResponseDto<T>> SendAsync(RequestDto request);
}
