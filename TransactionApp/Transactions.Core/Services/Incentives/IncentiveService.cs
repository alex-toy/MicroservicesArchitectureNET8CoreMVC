using Transactions.Core.Dtos;
using Transactions.Core.Dtos.Incentives;
using static Transactions.Core.Utils.Constants;

namespace Transactions.Core.Services.Incentives;

public class IncentiveService : IIncentiveService
{
	private readonly IBaseService _baseService;
	private const string ApiUrl = "/api/Incentive/";

	public IncentiveService(IBaseService baseService)
	{
		_baseService = baseService;
	}

	public async Task<ResponseDto<List<IncentiveDto>>> GetAllAsync(FilterIncentiveDto filter)
	{
		RequestDto<FilterIncentiveDto> request = new()
		{
			Data = filter,
			ApiType = ApiType.POST,
			Url = BonusAPIBase + ApiUrl + "GetAll",
		};

		return await _baseService.SendAsync<FilterIncentiveDto, List<IncentiveDto>>(request);
	}

	public async Task<ResponseDto<IncentiveDto>> GetByIdAsync(int incentiveId)
	{
		RequestDto<int> request = new()
		{
			ApiType = ApiType.GET,
			Url = BonusAPIBase + ApiUrl + "GetById/" + incentiveId,
		};

		return await _baseService.SendAsync<int, IncentiveDto>(request);
	}

	public async Task<ResponseDto<IncentiveDto>> GetByCodeAsync(string incentiveCode)
	{
		RequestDto<string> request = new()
		{
			ApiType = ApiType.GET,
			Url = BonusAPIBase + ApiUrl + "GetByCode/" + incentiveCode,
		};

		return await _baseService.SendAsync<string, IncentiveDto>(request);
	}

	public async Task<ResponseDto<int>> CreateAsync(IncentiveDto incentive)
	{
		RequestDto<IncentiveDto> request = new RequestDto<IncentiveDto>
		{
			Data = incentive,
			ApiType = ApiType.POST,
			Url = BonusAPIBase + ApiUrl + "Create",
		};

		return await _baseService.SendAsync<IncentiveDto, int>(request);
	}

	public async Task<ResponseDto<int>> UpdateAsync(IncentiveDto incentive)
	{
		RequestDto<IncentiveDto> request = new RequestDto<IncentiveDto>
		{
			Data = incentive,
			ApiType = ApiType.PUT,
			Url = BonusAPIBase + ApiUrl + "Update",
		};

		return await _baseService.SendAsync<IncentiveDto, int>(request);
	}

	public async Task<ResponseDto<bool>> DeleteAsync(int incentiveId)
	{
		RequestDto<int> request = new()
		{
			ApiType = ApiType.DELETE,
			Url = BonusAPIBase + ApiUrl + "Delete/" + incentiveId,
		};

		return await _baseService.SendAsync<int, bool>(request);
	}

	public async Task<ResponseDto<bool>> DeleteManyAsync(DeleteIncentiveDto incentive)
	{
		RequestDto<DeleteIncentiveDto> request = new()
		{
			Data = incentive,
			ApiType = ApiType.DELETE,
			Url = BonusAPIBase + ApiUrl + "DeleteMany",
		};

		return await _baseService.SendAsync<DeleteIncentiveDto, bool>(request);
	}
}
