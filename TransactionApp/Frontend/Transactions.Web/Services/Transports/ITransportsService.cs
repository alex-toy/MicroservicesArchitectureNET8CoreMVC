using Transactions.Core.Dtos;
using Transactions.Core.Dtos.Transports;

namespace Transactions.Web.Services.Transports;

public interface ITransportsService
{
    Task<ResponseDto<List<TransportDto>>> GetAllAsync(FilterTransportDto filter);
    Task<ResponseDto<TransportDto>> GetByIdAsync(int incentiveId);
    Task<ResponseDto<TransportDto>> GetByCodeAsync(string incentiveCode);
    Task<ResponseDto<int>> CreateAsync(TransportDto incentive);
    Task<ResponseDto<int>> UpdateAsync(TransportDto incentive);
    Task<ResponseDto<bool>> DeleteAsync(int incentiveId);
    Task<ResponseDto<bool>> DeleteManyAsync(DeleteTransportDto incentive);
}