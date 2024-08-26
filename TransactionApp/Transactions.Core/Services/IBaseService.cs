using Transactions.Core.Dtos;

namespace Transactions.Core.Services;

public interface IBaseService
{
    Task<ResponseDto<TResponse>> SendAsync<TRequest, TResponse>(RequestDto<TRequest> request, bool withBearer = true);
}
