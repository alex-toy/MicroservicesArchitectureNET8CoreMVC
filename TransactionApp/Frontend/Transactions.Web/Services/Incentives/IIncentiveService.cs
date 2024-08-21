using Transactions.Web.Dtos;
using Transactions.Web.Dtos.Data;

namespace Transactions.Web.Services.Incentives;

public interface IIncentiveService
{
    Task<ResponseDto<List<IncentiveDto>>> GetAllAsync();
    Task<ResponseDto<IncentiveDto>> GetByIdAsync(int incentiveId);
    Task<ResponseDto<IncentiveDto>> GetByCodeAsync(string incentiveCode);
    Task<ResponseDto<int>> CreateAsync(IncentiveDto incentive);
    Task<ResponseDto<int>> UpdateAsync(IncentiveDto incentive);
    Task<ResponseDto<bool>> DeleteAsync(int incentiveId);
}