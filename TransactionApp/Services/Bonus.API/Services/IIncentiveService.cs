using Bonus.API.Dtos;
using Bonus.API.Models;

namespace Bonus.API.Services;

public interface IIncentiveService
{
    int Upsert(IncentiveDto incentiveDto);
    List<IncentiveDto> GetAll(FilterIncentiveDto filterIncentive);
    IncentiveDto? Get(Func<Incentive, bool> predicate);
    bool Delete(int incentiveId);
    bool DeleteMany(DeleteIncentiveDto incentive);
}