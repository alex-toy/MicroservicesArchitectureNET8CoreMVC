using Transactions.Web.Dtos;
using Transactions.Web.Dtos.Data;
using static Transactions.Web.Utils.Constants;

namespace Transactions.Web.Services.Incentives;

public class IncentiveService : IIncentiveService
{
    private readonly IBaseService _baseService;

    public IncentiveService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto<List<IncentiveDto>>> GetAllAsync()
    {
        RequestDto<object> request = new ()
        {
            ApiType = ApiType.GET,
            Url = BonusAPIBase + "/api/Incentive/GetAll",
        };

        return await _baseService.SendAsync<object, List<IncentiveDto>>(request);
    }

    public async Task<ResponseDto<IncentiveDto>> GetByIdAsync(int incentiveId)
    {
        RequestDto<int> request = new()
        {
            ApiType = ApiType.GET,
            Url = BonusAPIBase + "/api/Incentive/GetById/" + incentiveId,
        };

        return await _baseService.SendAsync<int, IncentiveDto>(request);
    }

    public async Task<ResponseDto<IncentiveDto>> GetByCodeAsync(string incentiveCode)
    {
        RequestDto<string> request = new()
        {
            ApiType = ApiType.GET,
            Url = BonusAPIBase + "/api/Incentive/GetByCode/" + incentiveCode,
        };

        return await _baseService.SendAsync<string, IncentiveDto>(request);
    }

    public async Task<ResponseDto<int>> CreateAsync(IncentiveDto incentive)
    {
        RequestDto<IncentiveDto> request = new RequestDto<IncentiveDto>
        {
            Data = incentive,
            ApiType = ApiType.POST,
            Url = BonusAPIBase + "/api/Incentive/Create",
        };

        return await _baseService.SendAsync<IncentiveDto, int>(request);
    }

    public async Task<ResponseDto<int>> UpdateAsync(IncentiveDto incentive)
    {
        RequestDto<IncentiveDto> request = new RequestDto<IncentiveDto>
        {
            Data = incentive,
            ApiType = ApiType.PUT,
            Url = BonusAPIBase + "/api/Incentive/Update",
        };

        return await _baseService.SendAsync<IncentiveDto, int>(request);
    }

    public async Task<ResponseDto<bool>> DeleteAsync(int incentiveId)
    {
        RequestDto<int> request = new()
        {
            ApiType = ApiType.DELETE,
            Url = BonusAPIBase + "/api/Incentive/Delete/" + incentiveId,
        };

        return await _baseService.SendAsync<int, bool>(request);
    }
}
