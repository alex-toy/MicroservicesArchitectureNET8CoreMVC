using Transactions.Web.Dtos;

namespace Transactions.Web.Services;

public interface IBaseService
{
    Task<ResponseDto<TResponse>> SendAsync<TRequest, TResponse>(RequestDto<TRequest> request);
}
