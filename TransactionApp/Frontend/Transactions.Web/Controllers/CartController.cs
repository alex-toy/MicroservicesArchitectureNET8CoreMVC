using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using Transactions.Core.Dtos;
using Transactions.Core.Dtos.TransportCarts;
using Transactions.Web.Services.Carts;

namespace Transactions.Web.Controllers;

public class CartController : Controller
{
	private readonly ICartService _cartService;
	//private readonly IOrderService _orderService;

	public CartController(ICartService cartService)
	{
		_cartService = cartService;
	}

	[Authorize]
	public async Task<IActionResult> CartIndex()
    {
        string? userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
        CartDto cartDto =  await _cartService.GetUserCartDto(userId);
        return View(cartDto);
	}

	[Authorize]
	public async Task<IActionResult> Checkout()
    {
        string? userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
        CartDto cartDto = await _cartService.GetUserCartDto(userId);
        return View(cartDto);
	}

	//[HttpPost]
	//[ActionName("Checkout")]
	//public async Task<IActionResult> Checkout(CartDto cartDto)
	//{

	//	CartDto cart = await LoadCartDtoBasedOnLoggedInUser();
	//	cart.CartHeader.Phone = cartDto.CartHeader.Phone;
	//	cart.CartHeader.Email = cartDto.CartHeader.Email;
	//	cart.CartHeader.Name = cartDto.CartHeader.Name;

	//	var response = await _orderService.CreateOrder(cart);
	//	OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

	//	if (response != null && response.IsSuccess)
	//	{
	//		//get stripe session and redirect to stripe to place order
	//		//
	//		var domain = Request.Scheme + "://" + Request.Host.Value + "/";

	//		StripeRequestDto stripeRequestDto = new()
	//		{
	//			ApprovedUrl = domain + "cart/Confirmation?orderId=" + orderHeaderDto.OrderHeaderId,
	//			CancelUrl = domain + "cart/checkout",
	//			OrderHeader = orderHeaderDto
	//		};

	//		var stripeResponse = await _orderService.CreateStripeSession(stripeRequestDto);
	//		StripeRequestDto stripeResponseResult = JsonConvert.DeserializeObject<StripeRequestDto>
	//									(Convert.ToString(stripeResponse.Result));
	//		Response.Headers.Add("Location", stripeResponseResult.StripeSessionUrl);
	//		return new StatusCodeResult(303);
	//	}

	//	return View();
	//}

	//public async Task<IActionResult> Confirmation(int orderId)
	//{
	//	ResponseDto? response = await _orderService.ValidateStripeSession(orderId);
	//	if (response != null & response.IsSuccess)
	//	{

	//		OrderHeaderDto orderHeader = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
	//		if (orderHeader.Status == SD.Status_Approved)
	//		{
	//			return View(orderId);
	//		}
	//	}
	//	//redirect to some error page based on status
	//	return View(orderId);
	//}

	public async Task<IActionResult> Remove(int cartDetailsId)
	{
        string? userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
		ResponseDto<bool> response = await _cartService.RemoveFromCartAsync(cartDetailsId);

        if (!response.IsSuccess) return View();

        TempData["success"] = "Transport removed successfully";
        return RedirectToAction(nameof(CartIndex));
	}

	[HttpPost]
	public async Task<IActionResult> ApplyIncentive(CartDto cartDto)
	{
		ResponseDto<bool> response = await _cartService.ApplyIncentiveAsync(cartDto);

        if (!response.IsSuccess) return View();

        TempData["success"] = "Incentive applied successfully";

        return RedirectToAction(nameof(CartIndex));
	}

	//[HttpPost]
	//public async Task<IActionResult> EmailCart(CartDto cartDto)
	//{
	//	CartDto cart = await LoadCartDtoBasedOnLoggedInUser();
	//	cart.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;
	//	ResponseDto? response = await _cartService.EmailCart(cart);
	//	if (response != null & response.IsSuccess)
	//	{
	//		TempData["success"] = "Email will be processed and sent shortly.";
	//		return RedirectToAction(nameof(CartIndex));
	//	}
	//	return View();
	//}

	[HttpPost]
	public async Task<IActionResult> RemoveIncentive(CartDto cartDto)
	{
		cartDto.CartHeader.IncentiveCode = "";
		ResponseDto<bool> response = await _cartService.ApplyIncentiveAsync(cartDto);

		if (!response.IsSuccess) return View();

        TempData["success"] = "Cart updated successfully";
        return RedirectToAction(nameof(CartIndex));
	}


	private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
	{
		string? userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

		if (string.IsNullOrEmpty(userId)) return new CartDto();

		ResponseDto<CartDto>? response = await _cartService.GetCartByUserIdAsnyc(userId);

		if (response is null || !response.IsSuccess) return new CartDto();

		CartDto? cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result)!);

		return cartDto ?? new CartDto();
	}
}
