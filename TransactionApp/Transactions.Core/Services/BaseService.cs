using Newtonsoft.Json;
using System.Net;
using System.Text;
using Transactions.Core.Dtos;
using Transactions.Core.Utils.Tokens;
using static Transactions.Core.Utils.Constants;

namespace Transactions.Core.Services;

public class BaseService : IBaseService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICookieToken _tokenProvider;

    public BaseService(IHttpClientFactory httpClient, ICookieToken tokenProvider)
    {
        _httpClientFactory = httpClient;
        _tokenProvider = tokenProvider;
    }

    public async Task<ResponseDto<TResponse>> SendAsync<TRequest, TResponse>(RequestDto<TRequest> request, bool withBearer = true)
    {
        try
        {
            HttpResponseMessage apiResponse = await GetApiResponse(request);

            return await GetResponseDto<TResponse>(apiResponse);
        }
        catch (Exception ex)
        {
            return new ResponseDto<TResponse>() { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }

    private async Task<HttpResponseMessage> GetApiResponse<TRequest>(RequestDto<TRequest> request, bool withBearer = true)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient("TransactionAPI");
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(request.Url),
            Content = GetSerializedData(request),
            Method = GetHttpMethod(request.ApiType),
        };
        message.Headers.Add("Accept", "application/json");

        if (withBearer) SetMessageHeadersWithTokenFromCookie(message);

        HttpResponseMessage? apiResponse = await httpClient.SendAsync(message);
        return apiResponse;
    }

    private void SetMessageHeadersWithTokenFromCookie(HttpRequestMessage message)
    {
        string? token = _tokenProvider.GetTokenFromCookie();
        message.Headers.Add("Authorization", $"Bearer {token}");
    }

    private static async Task<ResponseDto<TResponse>> GetResponseDto<TResponse>(HttpResponseMessage apiResponse)
    {
        return apiResponse.StatusCode switch
        {
            HttpStatusCode.NotFound => new ResponseDto<TResponse>() { IsSuccess = false, ErrorMessage = "NotFound" },
            HttpStatusCode.Forbidden => new ResponseDto<TResponse>() { IsSuccess = false, ErrorMessage = "Forbidden" },
            HttpStatusCode.Unauthorized => new ResponseDto<TResponse>() { IsSuccess = false, ErrorMessage = "Unauthorized" },
            HttpStatusCode.InternalServerError => new ResponseDto<TResponse>() { IsSuccess = false, ErrorMessage = "InternalServerError" },
            _ => await Get200ResponseDto<TResponse>(apiResponse)
        };
	}

	private static async Task<ResponseDto<TResponse>> Get200ResponseDto<TResponse>(HttpResponseMessage apiResponse)
	{
		string apiContent = await apiResponse.Content.ReadAsStringAsync();
		ResponseDto<TResponse>? response = JsonConvert.DeserializeObject<ResponseDto<TResponse>>(apiContent);
        string errorMessage = response!.ErrorMessage ?? string.Empty;
        return new ResponseDto<TResponse>() { IsSuccess = response!.IsSuccess, Result = response!.Result, ErrorMessage = errorMessage };
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
