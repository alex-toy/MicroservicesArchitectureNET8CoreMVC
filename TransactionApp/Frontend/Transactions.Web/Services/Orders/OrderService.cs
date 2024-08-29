using Transactions.Core.Dtos;
using Transactions.Core.Dtos.TransportCarts;
using Transactions.Core.Services;
using Transactions.Core.Utils;

namespace Transactions.Web.Services.Orders;

public class OrderService
{
	private readonly IBaseService _baseService;
	private const string ApiUrl = "/api/Order/";

	public OrderService(IBaseService baseService)
	{
		_baseService = baseService;
	}

	public async Task<ResponseDto<int>> CreateOrder(CartDto cartDto)
	{
		return await _baseService.SendAsync<CartDto, int>(new RequestDto<CartDto>()
		{
			ApiType = Constants.ApiType.POST,
			Data = cartDto,
			Url = Constants.OrderAPI + "CreateOrder"
		});
	}

	//public async Task<ResponseDto<int>> CreateStripeSession(StripeRequestDto stripeRequestDto)
	//{
	//	return await _baseService.SendAsync<StripeRequestDto, int>(new RequestDto<StripeRequestDto>()
	//	{
	//		ApiType = Constants.ApiType.POST,
	//		Data = stripeRequestDto,
	//		Url = Constants.OrderAPI+ "CreateStripeSession"
	//	});
	//}

	//public async Task<ResponseDto?> GetAllOrder(string? userId)
	//{
	//	return await _baseService.SendAsync<CartDto, bool>(new RequestDto()
	//	{
	//		ApiType = Constants.ApiType.GET,
	//		Url = Constants.OrderAPI + "GetOrders?userId=" + userId
	//	});
	//}

	//public async Task<ResponseDto?> GetOrder(int orderId)
	//{
	//	return await _baseService.SendAsync<CartDto, bool>(new RequestDto()
	//	{
	//		ApiType = Constants.ApiType.GET,
	//		Url = Constants.OrderAPI + "GetOrder/" + orderId
	//	});
	//}

	//public async Task<ResponseDto?> UpdateOrderStatus(int orderId, string newStatus)
	//{
	//	return await _baseService.SendAsync<CartDto, bool>(new RequestDto()
	//	{
	//		ApiType = Constants.ApiType.POST,
	//		Data = newStatus,
	//		Url = Constants.OrderAPI + "UpdateOrderStatus/" + orderId
	//	});
	//}

	//public async Task<ResponseDto?> ValidateStripeSession(int orderHeaderId)
	//{
	//	return await _baseService.SendAsync<CartDto, bool>(new RequestDto()
	//	{
	//		ApiType = Constants.ApiType.POST,
	//		Data = orderHeaderId,
	//		Url = Constants.OrderAPI + "ValidateStripeSession"
	//	});
	//}
}
