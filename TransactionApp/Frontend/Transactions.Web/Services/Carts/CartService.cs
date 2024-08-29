using Transactions.Core.Dtos;
using Transactions.Core.Dtos.TransportCarts;
using Transactions.Core.Services;
using Transactions.Core.Utils;

namespace Transactions.Web.Services.Carts;

public class CartService : ICartService
{
	private readonly IBaseService _baseService;

	private const string ApiUrl = "/api/TransportCart/";

	public CartService(IBaseService baseService)
	{
		_baseService = baseService;
	}

	public async Task<ResponseDto<bool>> ApplyCouponAsync(CartDto cartDto)
	{
		return await _baseService.SendAsync<CartDto, bool>(new RequestDto<CartDto>()
		{
			ApiType = Constants.ApiType.POST,
			Data = cartDto,
			Url = Constants.TransportCartAPI + ApiUrl + "ApplyCoupon"
		});
	}

	public async Task<ResponseDto<bool>> EmailCartAsync(CartDto cartDto)
	{
		return await _baseService.SendAsync<CartDto, bool>(new RequestDto<CartDto>()
		{
			ApiType = Constants.ApiType.POST,
			Data = cartDto,
			Url = Constants.TransportCartAPI + ApiUrl + "EmailCartRequest"
		});
	}

	public async Task<ResponseDto<CartDto>> GetCartByUserIdAsnyc(string userId)
	{
		return await _baseService.SendAsync<int, CartDto>(new RequestDto<int>()
		{
			ApiType = Constants.ApiType.GET,
			Url = Constants.TransportCartAPI + ApiUrl + "GetCart/" + userId
		});
	}

	public async Task<ResponseDto<bool>> RemoveFromCartAsync(int cartDetailsId)
	{
		return await _baseService.SendAsync<int, bool>(new RequestDto<int>()
		{
			ApiType = Constants.ApiType.POST,
			Data = cartDetailsId,
			Url = Constants.TransportCartAPI + ApiUrl + "RemoveCart"
		});
	}

	public async Task<ResponseDto<bool>> UpsertCartAsync(CartDto cartDto)
	{
		return await _baseService.SendAsync<CartDto, bool>(new RequestDto<CartDto>()
		{
			ApiType = Constants.ApiType.POST,
			Data = cartDto,
			Url = Constants.TransportCartAPI + ApiUrl + "CartUpsert"
		});
	}
}
