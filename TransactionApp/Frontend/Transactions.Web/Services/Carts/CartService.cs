using Transactions.Core.Services;

namespace Transactions.Web.Services.Carts;

public class CartService
{
	private readonly IBaseService _baseService;

	public CartService(IBaseService baseService)
	{
		_baseService = baseService;
	}

	//public async Task<ResponseDto<int>> ApplyCouponAsync(CartDto cartDto)
	//{
	//	return await _baseService.SendAsync(new RequestDto<CartDto>()
	//	{
	//		ApiType = Constants.ApiType.POST,
	//		Data = cartDto,
	//		Url = Constants.TransportCartAPI + "/api/cart/ApplyCoupon"
	//	});
	//}

	//public async Task<ResponseDto?> EmailCart(CartDto cartDto)
	//{
	//	return await _baseService.SendAsync(new RequestDto<CartDto>()
	//	{
	//		ApiType = Constants.ApiType.POST,
	//		Data = cartDto,
	//		Url = Constants.TransportCartAPI + "/api/cart/EmailCartRequest"
	//	});
	//}

	//public async Task<ResponseDto?> GetCartByUserIdAsnyc(string userId)
	//{
	//	return await _baseService.SendAsync(new RequestDto()
	//	{
	//		ApiType = Constants.ApiType.GET,
	//		Url = Constants.TransportCartAPI + "/api/cart/GetCart/" + userId
	//	});
	//}

	//public async Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId)
	//{
	//	return await _baseService.SendAsync(new RequestDto()
	//	{
	//		ApiType = Constants.ApiType.POST,
	//		Data = cartDetailsId,
	//		Url = Constants.TransportCartAPI + "/api/cart/RemoveCart"
	//	});
	//}

	//public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
	//{
	//	return await _baseService.SendAsync(new RequestDto<CartDto>()
	//	{
	//		ApiType = Constants.ApiType.POST,
	//		Data = cartDto,
	//		Url = Constants.TransportCartAPI + "/api/cart/CartUpsert"
	//	});
	//}
}
