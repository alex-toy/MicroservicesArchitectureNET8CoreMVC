using Newtonsoft.Json;
using Transactions.Core.Dtos;
using Transactions.Core.Dtos.Incentives;

namespace TransportCart.Api.Services.Incentives;

public class IncentiveService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IncentiveService(IHttpClientFactory clientFactory)
    {
        _httpClientFactory = clientFactory;
    }

    public async Task<IncentiveDto> Getincentive(string incentiveCode)
    {
        HttpClient client = _httpClientFactory.CreateClient("Incentive");
        HttpResponseMessage response = await client.GetAsync($"/api/Incentive/GetByCode/{incentiveCode}");
        string apiContet = await response.Content.ReadAsStringAsync();
        ResponseDto<IncentiveDto>? responseIncentive = JsonConvert.DeserializeObject<ResponseDto<IncentiveDto>>(apiContet);

        return JsonConvert.DeserializeObject<IncentiveDto>(Convert.ToString(responseIncentive.Result));
    }
}
