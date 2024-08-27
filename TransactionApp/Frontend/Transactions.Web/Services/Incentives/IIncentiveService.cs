using Transactions.Core.Dtos;
using Transactions.Web.Dtos.Incentives;

namespace Transactions.Web.Services.Incentives;

public interface IIncentiveService
{
    Task<ResponseDto<List<IncentiveDto>>> GetAllAsync(FilterIncentiveDto filter);
    Task<ResponseDto<IncentiveDto>> GetByIdAsync(int incentiveId);
    Task<ResponseDto<IncentiveDto>> GetByCodeAsync(string incentiveCode);
    Task<ResponseDto<int>> CreateAsync(IncentiveDto incentive);
    Task<ResponseDto<int>> UpdateAsync(IncentiveDto incentive);
    Task<ResponseDto<bool>> DeleteAsync(int incentiveId);
    Task<ResponseDto<bool>> DeleteManyAsync(DeleteIncentiveDto incentive);
}