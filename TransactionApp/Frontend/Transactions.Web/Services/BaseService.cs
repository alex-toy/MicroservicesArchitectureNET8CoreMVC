using Newtonsoft.Json;
using System.Net;
using System.Text;
using Transactions.Web.Dtos;
using static Transactions.Web.Utils.Constants;

namespace Transactions.Web.Services;

public class BaseService : IBaseService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BaseService(IHttpClientFactory httpClient)
    {
        _httpClientFactory = httpClient;
    }

    public async Task<ResponseDto<TResponse>> SendAsync<TRequest, TResponse>(RequestDto<TRequest> request)
    {
        try
        {
            HttpResponseMessage apiResponse = await GetApiResponse(request);

            return  await GetResponseDto<TResponse>(apiResponse);
        }
        catch (Exception ex)
        {
            return new ResponseDto<TResponse>() { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }

    private async Task<HttpResponseMessage> GetApiResponse<TRequest>(RequestDto<TRequest> request)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient("TransactionAPI");
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(request.Url),
            Content = GetSerializedData(request),
            Method = GetHttpMethod(request.ApiType),
        };
        message.Headers.Add("Accept", "application/json");

        // Token

        HttpResponseMessage? apiResponse = await httpClient.SendAsync(message);
        return apiResponse;
    }

    private static async Task<ResponseDto<TResponse>> GetResponseDto<TResponse>(HttpResponseMessage apiResponse)
    {
        return apiResponse.StatusCode switch
        {
            HttpStatusCode.NotFound => new ResponseDto<TResponse>() { IsSuccess = false, ErrorMessage = "NotFound" },
            HttpStatusCode.Forbidden => new ResponseDto<TResponse>() { IsSuccess = false, ErrorMessage = "Forbidden" },
            HttpStatusCode.Unauthorized => new ResponseDto<TResponse>() { IsSuccess = false, ErrorMessage = "Unauthorized" },
            HttpStatusCode.InternalServerError => new ResponseDto<TResponse>() { IsSuccess = false, ErrorMessage = "InternalServerError" },
            _ => new ResponseDto<TResponse>() { IsSuccess = true, Result = await GetResult<TResponse>(apiResponse) },
        };
    }

    private static async Task<TResponse?> GetResult<TResponse>(HttpResponseMessage apiResponse)
    {
        string apiContent = await apiResponse.Content.ReadAsStringAsync();
        ResponseDto<TResponse>? response = JsonConvert.DeserializeObject<ResponseDto<TResponse>>(apiContent);
        return response!.Result;
    }

    private static StringContent GetSerializedData<TRequest>(RequestDto<TRequest> request)
    {
        if (string.IsNullOrEmpty(request.Url)) return null;
        return new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
    }

    private static HttpMethod GetHttpMethod(ApiType apiType)
    {
        return apiType switch
        {
            ApiType.POST => HttpMethod.Post,
            ApiType.GET => HttpMethod.Get,
            ApiType.PUT => HttpMethod.Put,
            ApiType.DELETE => HttpMethod.Delete,
            _ => HttpMethod.Get,
        };
    }
}
