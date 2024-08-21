using Transactions.Web.Dtos;

namespace Transactions.Web.Services.Incentives;

public interface IIncentiveService
{
    Task<ResponseDto<List<IncentiveDto>>> GetAllAsync();
    Task<ResponseDto<IncentiveDto>> GetByIdAsync(int incentiveId);
    Task<ResponseDto<IncentiveDto>> GetByCodeAsync(string incentiveCode);
    Task<ResponseDto<int>> Create(IncentiveDto incentive);
    Task<ResponseDto<int>> Update(IncentiveDto incentive);
    Task<ResponseDto<bool>> Delete(int incentiveId);
}