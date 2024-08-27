using Transactions.Core.Dtos;
using Transactions.Core.Dtos.Transports;
using static Transactions.Core.Utils.Constants;

namespace Transactions.Core.Services.Transports;

public class TransportsService : ITransportsService
{
	private readonly IBaseService _baseService;
	private const string ApiUrl = "/api/Transport/";

	public TransportsService(IBaseService baseService)
	{
		_baseService = baseService;
	}

	public async Task<ResponseDto<List<TransportDto>>> GetAllAsync(FilterTransportDto filter)
	{
		RequestDto<FilterTransportDto> request = new()
		{
			Data = filter,
			ApiType = ApiType.POST,
			Url = TransportAPIBase + ApiUrl + "GetAll",
		};

		return await _baseService.SendAsync<FilterTransportDto, List<TransportDto>>(request);
	}

	public async Task<ResponseDto<TransportDto>> GetByIdAsync(int incentiveId)
	{
		RequestDto<int> request = new()
		{
			ApiType = ApiType.GET,
			Url = TransportAPIBase + ApiUrl + "GetById/" + incentiveId,
		};

		return await _baseService.SendAsync<int, TransportDto>(request);
	}

	public async Task<ResponseDto<TransportDto>> GetByCodeAsync(string incentiveCode)
	{
		RequestDto<string> request = new()
		{
			ApiType = ApiType.GET,
			Url = TransportAPIBase + ApiUrl + "GetByCode/" + incentiveCode,
		};

		return await _baseService.SendAsync<string, TransportDto>(request);
	}

	public async Task<ResponseDto<int>> CreateAsync(TransportDto incentive)
	{
		RequestDto<TransportDto> request = new RequestDto<TransportDto>
		{
			Data = incentive,
			ApiType = ApiType.POST,
			Url = TransportAPIBase + ApiUrl + "Create",
		};

		return await _baseService.SendAsync<TransportDto, int>(request);
	}

	public async Task<ResponseDto<int>> UpdateAsync(TransportDto incentive)
	{
		RequestDto<TransportDto> request = new RequestDto<TransportDto>
		{
			Data = incentive,
			ApiType = ApiType.PUT,
			Url = TransportAPIBase + ApiUrl + "Update",
		};

		return await _baseService.SendAsync<TransportDto, int>(request);
	}

	public async Task<ResponseDto<bool>> DeleteAsync(int incentiveId)
	{
		RequestDto<int> request = new()
		{
			ApiType = ApiType.DELETE,
			Url = TransportAPIBase + ApiUrl + "Delete/" + incentiveId,
		};

		return await _baseService.SendAsync<int, bool>(request);
	}

	public async Task<ResponseDto<bool>> DeleteManyAsync(DeleteTransportDto incentive)
	{
		RequestDto<DeleteTransportDto> request = new()
		{
			Data = incentive,
			ApiType = ApiType.DELETE,
			Url = TransportAPIBase + ApiUrl + "DeleteMany",
		};

		return await _baseService.SendAsync<DeleteTransportDto, bool>(request);
	}
}
