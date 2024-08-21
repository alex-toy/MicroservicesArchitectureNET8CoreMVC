using Newtonsoft.Json;
using System.Net;
using System.Text;
using Transactions.Web.Dtos;
using static Transactions.Web.Dtos.Constants;

namespace Transactions.Web.Services;

public class BaseService<T> : IBaseService<T>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BaseService(IHttpClientFactory httpClient)
    {
        _httpClientFactory = httpClient;
    }

    public async Task<ResponseDto<T>> SendAsync(RequestDto request)
    {
        try
        {
            HttpResponseMessage apiResponse = await GetApiResponse(request);

            return  await GetResponseDto(apiResponse);
        }
        catch (Exception ex)
        {
            return new ResponseDto<T>() { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }

    private async Task<HttpResponseMessage> GetApiResponse(RequestDto request)
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

    private static async Task<ResponseDto<T>> GetResponseDto(HttpResponseMessage apiResponse)
    {
        return apiResponse.StatusCode switch
        {
            HttpStatusCode.NotFound => new ResponseDto<T>() { IsSuccess = false, ErrorMessage = "NotFound" },
            HttpStatusCode.Forbidden => new ResponseDto<T>() { IsSuccess = false, ErrorMessage = "Forbidden" },
            HttpStatusCode.Unauthorized => new ResponseDto<T>() { IsSuccess = false, ErrorMessage = "Unauthorized" },
            HttpStatusCode.InternalServerError => new ResponseDto<T>() { IsSuccess = false, ErrorMessage = "InternalServerError" },
            _ => new ResponseDto<T>() { IsSuccess = true, Result = await GetResult(apiResponse) },
        };
    }

    private static async Task<T?> GetResult(HttpResponseMessage apiResponse)
    {
        string apiContent = await apiResponse.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(apiContent);
    }

    private static StringContent GetSerializedData(RequestDto request)
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
